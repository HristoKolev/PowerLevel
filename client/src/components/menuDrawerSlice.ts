import { createSlice, PayloadAction, Selector } from '@reduxjs/toolkit';

import { RootState } from '~infrastructure/redux-store';

export enum ResponsiveDrawerBreakpoint {
  Overlaid = 900,
  SideBySide = Number.MAX_SAFE_INTEGER,
}

interface MenuDrawerState {
  open: boolean;
  breakpoint: ResponsiveDrawerBreakpoint | undefined;
}

const initialState: MenuDrawerState = {
  open: false,
  breakpoint: undefined,
};

interface ChangeDrawerBreakpoint {
  breakpoint: ResponsiveDrawerBreakpoint;
}

export const menuDrawerSlice = createSlice({
  name: 'MENU_DRAWER',
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

const menuDrawerSelector: Selector<RootState, MenuDrawerState> = (state) =>
  state[menuDrawerSlice.name];

export const selectors = {
  menuDrawerSelector,
};

export const actions = menuDrawerSlice.actions;
