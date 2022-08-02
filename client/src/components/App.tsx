import { memo, StrictMode, Fragment } from 'react';
import { CssBaseline } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';

import { StoreType, useAppSelector } from '~infrastructure/redux';
import { HomePage } from '~components/HomePage';
import { NotFoundPage } from '~components/NotFoundPage';
import { SignInPage } from '~components/SignInPage';
import { Layout } from '~layout';
import { isLoggedInSelector } from '~infrastructure/sessionSlice';

interface AppProps {
  store: StoreType;
}

const RoutesWrapper = memo(() => {
  const isLoggedIn = useAppSelector(isLoggedInSelector);

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />

      {!isLoggedIn && (
        <Fragment>
          <Route path="sign-in" element={<SignInPage />} />
        </Fragment>
      )}

      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
});

export const App = memo(({ store }: AppProps) => (
  <StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <CssBaseline />
        <Layout>
          <RoutesWrapper />
        </Layout>
      </BrowserRouter>
    </Provider>
  </StrictMode>
));
