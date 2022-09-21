import {
  createSlice,
  Selector,
  createSelector,
  PayloadAction,
  createAsyncThunk,
} from '@reduxjs/toolkit';

import { LoginResponse } from '~infra/RpcClient';
import { createRpcClient } from '~infra/create-rpc-client';
import { ReduxState } from '~infra/redux';

const logout = createAsyncThunk(
  `SESSION/logout`,
  (_, { getState, dispatch }) => {
    const rpcClient = createRpcClient(getState(), dispatch);

    // Ignore failed state
    void rpcClient.logoutResult();
  }
);

interface SessionState {
  loggedIn: boolean;
  userInfo: LoginResponse;
}

const initialState: SessionState = {
  loggedIn: false,
  userInfo: {} as LoginResponse,
};

export const sessionSlice = createSlice({
  name: 'SESSION',
  initialState,
  reducers: {
    login(state, { payload }: PayloadAction<LoginResponse>) {
      state.loggedIn = true;
      state.userInfo = payload;
    },
    silentLogout(state) {
      state.loggedIn = false;
      state.userInfo = {} as LoginResponse;
    },
  },
  extraReducers: {
    [logout.fulfilled.toString()]: (state) => {
      state.loggedIn = false;
      state.userInfo = {} as LoginResponse;
    },
  },
});

const sessionStateSelector: Selector<ReduxState, SessionState> = (state) =>
  state[sessionSlice.name];

export const isLoggedInSelector = createSelector(
  sessionStateSelector,
  (x) => x.loggedIn
);

export const csrfTokenSelector = createSelector(
  sessionStateSelector,
  (x) => x.userInfo.csrfToken
);

export const sessionActions = sessionSlice.actions;

export const sessionThunks = {
  logout,
};
