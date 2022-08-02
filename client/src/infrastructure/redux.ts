import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { layoutSlice } from '~layout';
import { RpcClient, BaseRpcClient } from '~rpc';

import { sessionSlice } from './sessionSlice';

export const createStore = (preloadedState?: unknown) =>
  configureStore({
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-explicit-any
    preloadedState: preloadedState as any,
    reducer: {
      [sessionSlice.name]: sessionSlice.reducer,
      [layoutSlice.name]: layoutSlice.reducer,
    },
  });

export type StoreType = ReturnType<typeof createStore>;

export type RootState = ReturnType<ReturnType<typeof createStore>['getState']>;

export const useAppDispatch = () =>
  useDispatch<ReturnType<typeof createStore>['dispatch']>();

export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const createRpcClient = (rootStateOrToken?: unknown): RpcClient => {
  let csrfToken;

  if (rootStateOrToken) {
    if (typeof rootStateOrToken === 'string') {
      csrfToken = rootStateOrToken;
    } else {
      const sessionState = (rootStateOrToken as RootState).SESSION;

      if (sessionState.loggedIn) {
        csrfToken = sessionState.userInfo.csrfToken;
      }
    }
  }

  const baseRpcClient = new BaseRpcClient();

  if (csrfToken) {
    baseRpcClient.setCSRFToken(csrfToken);
  }

  return new RpcClient(baseRpcClient);
};
