import { memo, StrictMode } from 'react';
import { CssBaseline } from '@mui/material';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Provider } from 'react-redux';

import { useAppSelector, ReduxStoreType } from '~infra/redux';
import { SignInPage } from '~auth/SignInPage';
import { isLoggedInSelector } from '~auth/sessionSlice';
import { Layout } from '~layout/Layout';
import { QuizList } from '~quizzes/QuizList';

export const NotFoundPage = memo((): JSX.Element => <div>Not found</div>);

const RoutesWrapper = memo(() => {
  const isLoggedIn = useAppSelector(isLoggedInSelector);

  return (
    <Routes>
      {isLoggedIn && (
        <>
          <Route path="/" element={<Navigate to="/quizzes" replace />} />
          <Route path="/quizzes" element={<QuizList />} />
          <Route path="*" element={<Navigate to="/quizzes" replace />} />
        </>
      )}

      {!isLoggedIn && (
        <>
          <Route path="sign-in" element={<SignInPage />} />
        </>
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
