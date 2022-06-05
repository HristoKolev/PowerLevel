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

if (window.__browserSupported) {
  if ('serviceWorker' in navigator) {
    void navigator.serviceWorker.register('service-worker.js');
  }

  const store = createStore();

  const root = document.getElementById('root') as Element;

  createDrawerResizeObserver(root, (breakpoint) => {
    store.dispatch(menuDrawerActions.changeDrawerBreakpoint({ breakpoint }));
  });

  createRoot(root).render(<App store={store} />);
}
