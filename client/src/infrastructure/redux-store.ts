import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { globalSlice } from '~components/globalSlice';
import { menuDrawerSlice } from '~components/menuDrawerSlice';

export const createStore = () =>
  configureStore({
    reducer: {
      [globalSlice.name]: globalSlice.reducer,
      [menuDrawerSlice.name]: menuDrawerSlice.reducer,
    },
  });

export type StoreType = ReturnType<typeof createStore>;

export type RootState = ReturnType<ReturnType<typeof createStore>['getState']>;

export const useAppDispatch = () =>
  useDispatch<ReturnType<typeof createStore>['dispatch']>();

export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
