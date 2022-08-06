import {
  createSlice,
  Selector,
  createSelector,
  PayloadAction,
  createAsyncThunk,
} from '@reduxjs/toolkit';

import { LoginResponse } from '~rpc';
import { RootState } from '~infrastructure/redux';
import { createRpcClient } from '~infrastructure/create-rpc-client';

const SESSION_SLICE_KEY = 'SESSION';

interface SessionState {
  loggedIn: boolean;
  userInfo: LoginResponse;
}

const initialState: SessionState = {
  loggedIn: false,
  userInfo: {} as LoginResponse,
};

const logout = createAsyncThunk(
  `${SESSION_SLICE_KEY}/logout`,
  async (_, { getState }) => {
    const rpcClient = createRpcClient(getState());

    // Ignore the failed state
    void rpcClient.logoutResult();
  }
);

interface LoginPayload {
  loginResponse: LoginResponse;
}

export const sessionSlice = createSlice({
  name: 'SESSION',
  initialState,
  reducers: {
    login(state, { payload }: PayloadAction<LoginPayload>) {
      state.loggedIn = true;
      state.userInfo = payload.loginResponse;
    },
  },
  extraReducers: {
    [logout.fulfilled.toString()]: (state) => {
      state.loggedIn = false;
      state.userInfo = {} as LoginResponse;
    },
  },
});

const sessionStateSelector: Selector<RootState, SessionState> = (state) =>
  state[sessionSlice.name];

export const isLoggedInSelector = createSelector(
  sessionStateSelector,
  (x) => x.loggedIn
);

export const sessionActions = sessionSlice.actions;

export const sessionThunks = {
  logout,
};
