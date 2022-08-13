import { createSlice, PayloadAction, Selector } from '@reduxjs/toolkit';

import { ReduxState } from '~infra/redux';

export enum ResponsiveDrawerBreakpoint {
  Overlaid = 900,
  SideBySide = Number.MAX_SAFE_INTEGER,
}

interface LayoutState {
  open: boolean;
  breakpoint: ResponsiveDrawerBreakpoint;
}

const initialState: LayoutState = {
  open: false,
  breakpoint: ResponsiveDrawerBreakpoint.Overlaid,
};

export const layoutSlice = createSlice({
  name: 'LAYOUT',
  initialState,
  extraReducers: {},
  reducers: {
    toggleDrawer(state) {
      state.open = !state.open;
    },
    changeDrawerBreakpoint(
      state,
      { payload }: PayloadAction<ResponsiveDrawerBreakpoint>
    ) {
      state.breakpoint = payload;
    },
  },
});

export const layoutSelector: Selector<ReduxState, LayoutState> = (state) =>
  state[layoutSlice.name];

export const layoutActions = layoutSlice.actions;
