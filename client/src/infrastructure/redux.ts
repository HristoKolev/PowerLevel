import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { globalSlice } from '~components/globalSlice';
import { layoutSlice } from '~layout';

export const createStore = (preloadedState?: unknown) =>
  configureStore({
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-explicit-any
    preloadedState: preloadedState as any,
    reducer: {
      [globalSlice.name]: globalSlice.reducer,
      [layoutSlice.name]: layoutSlice.reducer,
    },
  });

export type StoreType = ReturnType<typeof createStore>;

export type RootState = ReturnType<ReturnType<typeof createStore>['getState']>;

export const useAppDispatch = () =>
  useDispatch<ReturnType<typeof createStore>['dispatch']>();

export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
