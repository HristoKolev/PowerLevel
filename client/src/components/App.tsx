import { memo, StrictMode } from 'react';
import { CssBaseline } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';

import { CustomAppBar } from '~components/CustomAppBar';
import { MainContent } from '~components/MainContent';
import { MenuDrawer } from '~components/MenuDrawer';
import { Home } from '~components/Home';
import { Work } from '~components/Work';
import { NotFound } from '~components/NotFound';
import { StoreType } from '~infrastructure/redux-store';

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
            <Route path="work" element={<Work />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </MainContent>
      </BrowserRouter>
    </Provider>
  </StrictMode>
));
