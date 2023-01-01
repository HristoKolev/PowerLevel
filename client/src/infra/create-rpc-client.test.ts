import { act } from '@testing-library/react';

import { createReduxStore, ReduxStoreType } from '~infra/redux';
import { BaseRpcClient } from '~infra/BaseRpcClient';
import { sessionActions } from '~auth/sessionSlice';
import { RpcClient } from '~infra/RpcClient';

import { createRpcClient } from './create-rpc-client';

const getCSRFToken = (rpcClient: RpcClient) => {
  const baseClient = Reflect.get(rpcClient, 'baseClient') as BaseRpcClient;
  expect(baseClient).toBeTruthy();
  return baseClient.getCSRFToken();
};

const loginUser = (store: ReduxStoreType, csrfToken: string) => {
  act(() => {
    store.dispatch(
      sessionActions.login({
        csrfToken,
        emailAddress: 'test@test.test',
        userProfileID: 1,
      })
    );
  });
};

test('token is present when called with root state and user is logged in', async () => {
  const store = createReduxStore();

  const testToken = '__TOKEN__';

  loginUser(store, testToken);

  const rpcClient = createRpcClient(store.getState());

  expect(getCSRFToken(rpcClient)).toEqual(testToken);
});

test('token is not present when called with root state and user is logged out', async () => {
  const store = createReduxStore();

  const rpcClient = createRpcClient(store.getState());

  expect(getCSRFToken(rpcClient)).toBeUndefined();
});

test('token is present when called with token directly', async () => {
  const store = createReduxStore();

  const testToken = '__TOKEN__';

  loginUser(store, testToken);

  const rpcClient = createRpcClient(testToken);

  expect(getCSRFToken(rpcClient)).toEqual(testToken);
});

test('token is not present when called without a parameter', async () => {
  const rpcClient = createRpcClient();

  expect(getCSRFToken(rpcClient)).toBeUndefined();
});
