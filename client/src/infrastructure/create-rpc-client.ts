import { RpcClient, BaseRpcClient } from '~rpc';
import { RootState } from '~infrastructure/redux';

// TODO: Find a better way to create an rpc client
export const createRpcClient = (rootStateOrToken?: unknown): RpcClient => {
  let csrfToken;

  if (rootStateOrToken) {
    if (typeof rootStateOrToken === 'string') {
      csrfToken = rootStateOrToken;
    } else {
      const sessionState = (rootStateOrToken as RootState).SESSION;

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
