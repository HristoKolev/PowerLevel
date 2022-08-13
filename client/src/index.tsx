import { createRoot } from 'react-dom/client';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import './styles.css';

import { App } from '~components/App';
import { sessionSlice } from '~auth/sessionSlice';
import { ReduxPersistManager } from '~infra/ReduxPersistManager';
import { layoutSlice } from '~layout/layoutSlice';
import { createReduxStore } from '~infra/redux';

declare global {
  interface Window {
    __browserSupported: boolean;
  }
}

if (window.__browserSupported) {
  if ('serviceWorker' in navigator) {
    void navigator.serviceWorker.register('service-worker.js');
  }

  const reduxPersistManager = new ReduxPersistManager(
    localStorage,
    'b1790b84-0f83-11ed-a424-6b51cdbb1fbe',
    [layoutSlice.name, sessionSlice.name]
  );

  const store = createReduxStore(reduxPersistManager.readPersistedState());

  reduxPersistManager.subscribe(store);

  const rootElement = document.getElementById('root') as Element;

  createRoot(rootElement).render(<App store={store} />);
}
