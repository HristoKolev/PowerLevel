import { createSlice, PayloadAction, Selector } from '@reduxjs/toolkit';

import { RootState } from '~infrastructure/redux';

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

interface ChangeDrawerBreakpoint {
  breakpoint: ResponsiveDrawerBreakpoint;
}

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
      { payload }: PayloadAction<ChangeDrawerBreakpoint>
    ) {
      state.breakpoint = payload.breakpoint;
    },
  },
});

export const layoutSelector: Selector<RootState, LayoutState> = (state) =>
  state[layoutSlice.name];

export const layoutActions = layoutSlice.actions;
