import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { layoutSlice } from '~layout/layoutSlice';
import { sessionSlice } from '~auth/sessionSlice';

export const createReduxStore = (preloadedState?: unknown) =>
  configureStore({
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-explicit-any
    preloadedState: preloadedState as any,
    reducer: {
      [sessionSlice.name]: sessionSlice.reducer,
      [layoutSlice.name]: layoutSlice.reducer,
    },
  });

// TODO: rename these interfaces
export type ReduxStoreType = ReturnType<typeof createReduxStore>;

export type ReduxState = ReturnType<
  ReturnType<typeof createReduxStore>['getState']
>;

export const useAppDispatch = () =>
  useDispatch<ReturnType<typeof createReduxStore>['dispatch']>();

export const useAppSelector: TypedUseSelectorHook<ReduxState> = useSelector;
