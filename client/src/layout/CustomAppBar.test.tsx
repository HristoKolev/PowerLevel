import { screen, waitFor, act } from '@testing-library/react';
import { useNavigate } from 'react-router-dom';
import userEvent from '@testing-library/user-event';

import { renderWithProviders } from '~infrastructure/test-utils';
import { CustomAppBar } from '~layout/CustomAppBar';
import { createStore, StoreType } from '~infrastructure/redux';
import { sessionActions } from '~infrastructure/sessionSlice';
import { createRpcClient } from '~infrastructure/create-rpc-client';
import { apiResult } from '~rpc';

jest.mock('react-router-dom', () => {
  const navigate = jest.fn();

  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => navigate,
  };
});

jest.mock('~infrastructure/create-rpc-client', () => {
  const proxy = new Proxy(new Map<string, unknown>(), {
    get(map, propName: string): unknown {
      if (!map.has(propName)) {
        map.set(propName, jest.fn());
      }
      return map.get(propName);
    },
  });

  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('~infrastructure/create-rpc-client'),
    createRpcClient: () => proxy,
  };
});

// eslint-disable-next-line @typescript-eslint/unbound-method
const logoutResultMock = createRpcClient().logoutResult as jest.Mock;

// eslint-disable-next-line react-hooks/rules-of-hooks
const navigateMock = useNavigate() as jest.Mock;

const loginUser = (store: StoreType) => {
  act(() => {
    store.dispatch(
      sessionActions.login({
        loginResponse: {
          csrfToken: '__TOKEN__',
          emailAddress: 'test@test.test',
          userProfileID: 1,
        },
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
  const store = createStore();
  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  await waitFor(() => {
    expect(screen.queryByTestId('sign-in-button')).not.toBeInTheDocument();
  });
});

test('sign out button logs out the user', async () => {
  const user = userEvent.setup();

  const store = createStore();

  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  logoutResultMock.mockImplementation(() => apiResult.ok<null>(null));

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

  const store = createStore();

  renderWithProviders(<CustomAppBar />, store);

  loginUser(store);

  logoutResultMock.mockImplementation(() =>
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
