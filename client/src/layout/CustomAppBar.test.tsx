import { screen, waitFor, act } from '@testing-library/react';
import { useNavigate } from 'react-router-dom';
import userEvent from '@testing-library/user-event';

import { renderWithProviders } from '~test-utils';
import { sessionActions } from '~auth/sessionSlice';
import { apiResult } from '~infra/api-result';
import { createReduxStore, ReduxStoreType } from '~infra/redux';
import { RpcClient } from '~infra/RpcClient';

import { CustomAppBar } from './CustomAppBar';

type RpcClientMockType = { [key in keyof RpcClient]: jest.Mock };

jest.mock('~infra/RpcClient', () => {
  const MockedRpcClient: RpcClientMockType = (() => {
    const map = new Map<string, jest.Mock>();
    const proxy: RpcClientMockType = new Proxy(
      function () {} as unknown as RpcClientMockType,
      {
        construct() {
          return proxy;
        },
        get(_, key: string): jest.Mock {
          if (!map.has(key)) {
            map.set(key, jest.fn());
          }
          return map.get(key) as jest.Mock;
        },
      }
    );
    return proxy;
  })();
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('~infra/RpcClient'),
    RpcClient: MockedRpcClient,
  };
});

const RpcClientMock = RpcClient as unknown as RpcClientMockType;

jest.mock('react-router-dom', () => {
  const navigate = jest.fn();
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => navigate,
  };
});

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

afterEach(() => {
  jest.resetAllMocks();
  jest.restoreAllMocks();
  jest.resetModules();
});

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
    Object {
      "loggedIn": false,
      "userInfo": Object {},
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
    Object {
      "loggedIn": false,
      "userInfo": Object {},
    }
  `);

  expect(navigateMock).toHaveBeenCalledWith('/');
});
