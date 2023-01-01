import { screen, waitFor, act } from '@testing-library/react';
import { useNavigate } from 'react-router-dom';
import userEvent from '@testing-library/user-event';

import { renderWithProviders } from '~test-utils';
import { sessionActions } from '~auth/sessionSlice';
import { apiResult } from '~infra/api-result';
import { createReduxStore, ReduxStoreType } from '~infra/redux';
import { RpcClient } from '~infra/RpcClient';

import { CustomAppBar } from './CustomAppBar';

jest.mock('~infra/RpcClient');
type RpcClientMockType = { [key in keyof RpcClient]: jest.Mock };
const RpcClientMock = RpcClient as unknown as RpcClientMockType;

jest.mock('react-router-dom');
// eslint-disable-next-line react-hooks/rules-of-hooks
const navigateMock = useNavigate() as jest.Mock;

const loginUser = (store: ReduxStoreType) => {
  act(() => {
    store.dispatch(
      sessionActions.login({
        csrfToken: '__TOKEN__',
        emailAddress: 'test@test.test',
        userProfileID: 1,
      })
    );
  });
};

test('login buttons shows when user is not logged in', async () => {
  renderWithProviders(<CustomAppBar />);
  expect(screen.getByTestId('sign-in-button')).toBeInTheDocument();
});

test('login button does not show when the user is logged in', async () => {
  const store = createReduxStore();
  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  await waitFor(() => {
    expect(screen.queryByTestId('sign-in-button')).not.toBeInTheDocument();
  });
});

test('sign out button logs out the user', async () => {
  const user = userEvent.setup();

  const store = createReduxStore();

  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  RpcClientMock.logoutResult.mockImplementation(() => apiResult.ok<null>(null));

  await user.click(await screen.findByTestId('sign-out-button'));

  expect(store.getState().SESSION).toMatchInlineSnapshot(`
    {
      "loggedIn": false,
      "userInfo": {},
    }
  `);

  expect(navigateMock).toHaveBeenCalledWith('/');
});

test('sign out button logs out the user even if the server request failed', async () => {
  const user = userEvent.setup();

  const store = createReduxStore();

  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  RpcClientMock.logoutResult.mockImplementation(() =>
    apiResult.error({ errorMessages: ['Failed to logout user.'] })
  );

  await user.click(await screen.findByTestId('sign-out-button'));

  expect(store.getState().SESSION).toMatchInlineSnapshot(`
    {
      "loggedIn": false,
      "userInfo": {},
    }
  `);

  expect(navigateMock).toHaveBeenCalledWith('/');
});
