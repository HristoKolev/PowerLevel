import { memo, StrictMode } from 'react';
import { CssBaseline } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';

import { StoreType } from '~infrastructure/redux';
import { HomePage } from '~components/HomePage';
import { NotFoundPage } from '~components/NotFoundPage';
import { LoginPage } from '~components/LoginPage';
import { Layout } from '~layout';

interface AppProps {
  store: StoreType;
}

export const App = memo(({ store }: AppProps) => (
  <StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <CssBaseline />
        <Layout>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Layout>
      </BrowserRouter>
    </Provider>
  </StrictMode>
));
