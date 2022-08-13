import { memo, StrictMode, Fragment } from 'react';
import { CssBaseline, Button, Alert, AlertTitle } from '@mui/material';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';

import { useAppSelector, ReduxStoreType } from '~infra/redux';
import { SignInPage } from '~auth/SignInPage';
import { isLoggedInSelector } from '~auth/sessionSlice';
import { Layout } from '~layout/Layout';

const HomePage = memo(
  (): JSX.Element => (
    <div>
      {Array(5)
        .fill(undefined)
        .map((_, i) => (
          <Fragment key={i}>
            <Button variant="contained">Contained</Button>
            <Alert severity="error">
              <AlertTitle>Error</AlertTitle>
              This is an error alert — <strong>check it out!</strong>
            </Alert>
          </Fragment>
        ))}
    </div>
  )
);

export const NotFoundPage = memo((): JSX.Element => <div>Not found</div>);

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

export interface AppProps {
  store: ReduxStoreType;
}

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
