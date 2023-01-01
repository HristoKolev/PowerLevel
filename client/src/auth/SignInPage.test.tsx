import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useNavigate } from 'react-router-dom';

import { renderWithProviders, WaitHandle } from '~test-utils';
import { createReduxStore } from '~infra/redux';
import { LoginResponse, RpcClient } from '~infra/RpcClient';
import { apiResult } from '~infra/api-result';

import { SignInPage } from './SignInPage';

jest.mock('~shared/RecaptchaField');

jest.mock('~infra/RpcClient');
type RpcClientMockType = { [key in keyof RpcClient]: jest.Mock };
const RpcClientMock = RpcClient as unknown as RpcClientMockType;

jest.mock('react-router-dom');
// eslint-disable-next-line react-hooks/rules-of-hooks
const navigateMock = useNavigate() as jest.Mock;

test('renders title and fields', async () => {
  renderWithProviders(<SignInPage />);

  expect(await screen.findByTestId('form-title')).toHaveTextContent('Sign in');

  await waitFor(() => {
    expect(screen.getByTestId('emailAddress')).toBeInTheDocument();
  });

  await waitFor(() => {
    expect(screen.getByTestId('password')).toBeInTheDocument();
  });

  await waitFor(() => {
    expect(screen.getByTestId('rememberMe')).toBeInTheDocument();
  });

  await waitFor(() => {
    expect(screen.getByTestId('recaptchaToken')).toBeInTheDocument();
  });
});

test('logs in successfully', async () => {
  const user = userEvent.setup();
  const store = createReduxStore();

  renderWithProviders(<SignInPage />, store);

  const emailAddressField = await screen.findByTestId('emailAddress');
  const passwordField = await screen.findByTestId('password');
  const rememberMeField = await screen.findByTestId('rememberMe');

  await user.type(emailAddressField, 'test@test.test');
  await user.type(passwordField, 'password');
  await user.click(rememberMeField);

  const loginResultWaitHandle = new WaitHandle();

  RpcClientMock.loginResult.mockImplementation(async () => {
    await loginResultWaitHandle.wait();
    return apiResult.ok<LoginResponse>({
      csrfToken: '__TOKEN__',
      emailAddress: 'test@test.test',
      userProfileID: 1,
    });
  });

  await user.click(await screen.findByTestId('submit-button'));

  expect(RpcClientMock.loginResult).toHaveBeenCalledWith({
    emailAddress: 'test@test.test',
    password: 'password',
    rememberMe: true,
  });

  // Loading state
  await waitFor(() => {
    expect(screen.getByTestId('submit-button')).toBeDisabled();
    expect(screen.getByTestId('loading-indicator')).toBeInTheDocument();
  });

  loginResultWaitHandle.release();

  await waitFor(() => {
    expect(store.getState().SESSION).toStrictEqual({
      loggedIn: true,
      userInfo: {
        csrfToken: '__TOKEN__',
        emailAddress: 'test@test.test',
        userProfileID: 1,
      },
    });
  });

  expect(navigateMock).toHaveBeenCalledWith('/');
});

test('shows error message on request failure', async () => {
  const user = userEvent.setup();

  renderWithProviders(<SignInPage />);

  const emailAddressField = await screen.findByTestId('emailAddress');
  const passwordField = await screen.findByTestId('password');
  const rememberMeField = await screen.findByTestId('rememberMe');

  await user.type(emailAddressField, 'test@test.test');
  await user.type(passwordField, 'password');
  await user.click(rememberMeField);

  const errorMessages = ['Error 1', 'Error 2'];

  const loginResultWaitHandle = new WaitHandle();

  RpcClientMock.loginResult.mockImplementation(async () => {
    await loginResultWaitHandle.wait();
    return apiResult.error({ errorMessages });
  });

  await user.click(await screen.findByTestId('submit-button'));

  expect(RpcClientMock.loginResult).toHaveBeenCalledWith({
    emailAddress: 'test@test.test',
    password: 'password',
    rememberMe: true,
  });

  // Loading state
  await waitFor(() => {
    expect(screen.getByTestId('submit-button')).toBeDisabled();
    expect(screen.getByTestId('loading-indicator')).toBeInTheDocument();
  });

  loginResultWaitHandle.release();

  await waitFor(async () => {
    const errorMessageElements = await screen.findAllByTestId('error-message');
    const screenErrorMessages = errorMessageElements.map((x) => x.innerHTML);
    expect(screenErrorMessages).toStrictEqual(errorMessages);
  });
});
