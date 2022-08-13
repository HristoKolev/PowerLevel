// TODO: Find a better way to create an rpc client

import { RpcClient } from './RpcClient';
import { ReduxState } from './redux';
import { BaseRpcClient } from './BaseRpcClient';

export const createRpcClient = (rootStateOrToken?: unknown): RpcClient => {
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

  if (csrfToken) {
    baseRpcClient.setCSRFToken(csrfToken);
  }

  return new RpcClient(baseRpcClient);
};
