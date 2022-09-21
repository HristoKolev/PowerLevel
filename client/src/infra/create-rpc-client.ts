// TODO: Find a better way to create an rpc client

import { sessionActions } from '~auth/sessionSlice';

import { RpcClient } from './RpcClient';
import { ReduxState, DispatchType } from './redux';
import { BaseRpcClient } from './BaseRpcClient';

export const createRpcClient = (
  rootStateOrToken?: unknown,
  dispatch?: unknown
): RpcClient => {
  let csrfToken;

  if (rootStateOrToken) {
    if (typeof rootStateOrToken === 'string') {
      csrfToken = rootStateOrToken;
    } else {
      const sessionState = (rootStateOrToken as ReduxState).SESSION;

      if (sessionState.loggedIn) {
        csrfToken = sessionState.userInfo.csrfToken;
      }
    }
  }

  const baseRpcClient = new BaseRpcClient();

  if (dispatch) {
    const dispatchFn = dispatch as DispatchType;
    baseRpcClient.onSessionRejected(() => {
      dispatchFn(sessionActions.silentLogout());
    });
  }

  if (csrfToken) {
    baseRpcClient.setCSRFToken(csrfToken);
  }

  return new RpcClient(baseRpcClient);
};
