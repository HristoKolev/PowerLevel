import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import { createStore, createRpcClient } from '~infrastructure/redux';
import { SignInPage } from '~components/SignInPage';
import { TestSetup } from '~infrastructure/test-utils';

jest.mock('~infrastructure/RecaptchaField', () => ({
  RecaptchaField: ({ testid }: { testid: string }) => (
    <div data-testid={testid} />
  ),
}));

jest.mock('~infrastructure/redux', () => {
  const map = new Map<string, unknown>();
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('~infrastructure/redux'),
    createRpcClient: () =>
      new Proxy(
        {},
        {
          get(_, key: string): unknown {
            if (!map.has(key)) {
              map.set(key, jest.fn());
            }
            return map.get(key);
          },
        }
      ),
  };
});

// eslint-disable-next-line @typescript-eslint/unbound-method
const loginResultMock = createRpcClient().loginResult as jest.Mock;

describe('<SignInPage />', () => {
  afterEach(() => {
    jest.resetAllMocks();
    jest.restoreAllMocks();
    jest.resetModules();
  });

  test('renders title and fields', async () => {
    const store = createStore();

    render(
      <TestSetup store={store}>
        <SignInPage />
      </TestSetup>
    );

    const formTitle = await screen.findByTestId('form-title');
    expect(formTitle).toHaveTextContent('Sign in');

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

    const store = createStore();

    render(
      <TestSetup store={store}>
        <SignInPage />
      </TestSetup>
    );

    const emailAddressField = await screen.findByTestId('emailAddress');
    const passwordField = await screen.findByTestId('password');
    const rememberMeField = await screen.findByTestId('rememberMe');

    await user.type(emailAddressField, 'test@test.test');
    await user.type(passwordField, 'password');

    await user.click(rememberMeField);

    loginResultMock.mockImplementation(() => Promise.resolve({ isOk: false }));

    await user.click(await screen.findByTestId('submit-button'));

    expect(loginResultMock).toBeCalledWith({});
  });
});
