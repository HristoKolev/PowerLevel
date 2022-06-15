import { createRoot } from 'react-dom/client';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import './styles.css';

import { createStore } from '~infrastructure/redux-store';
import { App } from '~components/App';
import { createDrawerResizeObserver } from '~layout/createDrawerResizeObserver';
import { menuDrawerActions } from '~layout/menuDrawerSlice';
import { RpcClientHelper } from '~infrastructure/RpcClientHelper';
import { ApiResult, RpcClient } from '~infrastructure/RpcClient';

function unwrapResult<T>(result: ApiResult<T>): T {
  if (!result.isOk) {
    throw new Error(result.error.errorMessages.join(', '));
  }

  return result.payload;
}

if (window.__browserSupported) {
  if ('serviceWorker' in navigator) {
    void navigator.serviceWorker.register('service-worker.js');
  }

  void (async () => {
    const rpcHelper = new RpcClientHelper();
    const rpcClient = new RpcClient(rpcHelper);

    const loginResult = unwrapResult(
      await rpcClient.login({
        emailAddress: 'test@test.test',
        password: 'test@test.test',
        rememberMe: false,
      })
    );

    rpcHelper.setCSRFToken(loginResult.csrfToken);

    unwrapResult(await rpcClient.profileInfo({ count: 2 }));
    // eslint-disable-next-line no-console
  })().catch(console.error);

  const store = createStore();

  const root = document.getElementById('root') as Element;

  createDrawerResizeObserver(root, (breakpoint) => {
    store.dispatch(menuDrawerActions.changeDrawerBreakpoint({ breakpoint }));
  });

  createRoot(root).render(<App store={store} />);
}
