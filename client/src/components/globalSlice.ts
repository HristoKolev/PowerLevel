import { createSlice, Selector } from '@reduxjs/toolkit';

import { RootState } from '~infrastructure/redux';

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

export const globalSelector: Selector<RootState, GlobalState> = (state) =>
  state[globalSlice.name];

export const globalActions = globalSlice.actions;
