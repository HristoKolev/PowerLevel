import { memo, StrictMode } from 'react';
import { CssBaseline } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';

import { CustomAppBar } from '~layout/CustomAppBar';
import { MainContent } from '~layout/MainContent';
import { MenuDrawer } from '~layout/MenuDrawer';
import { Home } from '~components/Home';
import { NotFound } from '~components/NotFound';
import { StoreType } from '~infrastructure/redux-store';
import { LoginPage } from '~components/LoginPage';

interface AppProps {
  store: StoreType;
}

export const App = memo(({ store }: AppProps) => (
  <StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <CssBaseline />
        <CustomAppBar />
        <MenuDrawer />
        <MainContent>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </MainContent>
      </BrowserRouter>
    </Provider>
  </StrictMode>
));
