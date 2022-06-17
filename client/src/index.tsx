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
import { BaseRpcClient, RpcClient } from '~rpc';

if (window.__browserSupported) {
  if ('serviceWorker' in navigator) {
    void navigator.serviceWorker.register('service-worker.js');
  }

  void (async () => {
    const rpcHelper = new BaseRpcClient();
    const rpcClient = new RpcClient(rpcHelper);

    const loginResponse = await rpcClient.login({
      emailAddress: 'test@test.test',
      password: 'test@test.test',
      rememberMe: false,
    });

    // eslint-disable-next-line no-console
    console.log(loginResponse);

    rpcHelper.setCSRFToken(loginResponse.csrfToken);

    const profileInfoResponse = await rpcClient.profileInfo({ count: 2 });

    // eslint-disable-next-line no-console
    console.log(profileInfoResponse);

    // eslint-disable-next-line no-console
  })().catch(console.error);

  const store = createStore();

  const root = document.getElementById('root') as Element;

  createDrawerResizeObserver(root, (breakpoint) => {
    store.dispatch(menuDrawerActions.changeDrawerBreakpoint({ breakpoint }));
  });

  createRoot(root).render(<App store={store} />);
}
