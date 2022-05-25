import { createSlice, Selector } from '@reduxjs/toolkit';

import { RootState } from '~infrastructure/redux-store';

interface GlobalState {
  count: number;
}

const initialState: GlobalState = {
  count: 0,
};

export const globalSlice = createSlice({
  name: 'GLOBAL',
  initialState,
  extraReducers: {},
  reducers: {},
});

const globalSelector: Selector<RootState, GlobalState> = (state) =>
  state[globalSlice.name];

export const selectors = {
  globalSelector,
};
