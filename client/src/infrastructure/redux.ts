import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { layoutSlice } from '~layout';

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

// TODO: rename these interfaces
export type StoreType = ReturnType<typeof createStore>;

export type RootState = ReturnType<ReturnType<typeof createStore>['getState']>;

export const useAppDispatch = () =>
  useDispatch<ReturnType<typeof createStore>['dispatch']>();

export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
