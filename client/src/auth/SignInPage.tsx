import {
  memo,
  useState,
  useCallback,
  useMemo,
  InputHTMLAttributes,
} from 'react';
import { css } from '@linaria/core';
import {
  Button,
  Paper,
  TextField,
  Alert,
  Switch,
  FormControlLabel,
} from '@mui/material';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';

import { breakpoints } from '~infra/helpers';
import { rpcValidations } from '~infra/rpc-validations';
import { useAppDispatch } from '~infra/redux';
import { ApiResult } from '~infra/api-result';
import { LoginRequest, LoginResponse, LoginError } from '~infra/RpcClient';
import { sessionActions } from '~auth/sessionSlice';
import { RecaptchaField } from '~shared/RecaptchaField';
import { LoadingIndicator } from '~shared/LoadingIndicator';
import { createRpcClient } from '~infra/create-rpc-client';

const signInPageClassName = css`
  form {
    width: 336px;

    @media (min-width: ${breakpoints.tablet}px) {
      width: 400px;
    }
  }

  .title {
    font-size: 1.4rem;
  }
`;

const validations = rpcValidations.loginRequest;

export const SignInPage = memo((): JSX.Element => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [serverResult, setServerResult] = useState<
    ApiResult<LoginResponse, LoginError> | undefined
  >();

  const [submitLoading, setSubmitLoading] = useState(false);

  const onSubmit = useCallback(
    async (formValues: LoginRequest) => {
      setSubmitLoading(true);
      setServerResult(undefined);

      const rpcClient = createRpcClient(undefined, dispatch);

      const result = await rpcClient.loginResult(formValues);

      setServerResult(result);
      setSubmitLoading(false);

      if (result.isOk) {
        dispatch(sessionActions.login(result.payload));
        navigate('/');
      }
    },
    [dispatch, navigate]
  );

  const { register, handleSubmit, control, formState } = useForm<LoginRequest>({
    mode: 'onTouched',
  });

  const handleOnSubmit = useMemo(
    () => handleSubmit(onSubmit),
    [onSubmit, handleSubmit]
  );

  return (
    <div className={`flex justify-around ${signInPageClassName}`}>
      {/* eslint-disable-next-line @typescript-eslint/no-misused-promises */}
      <form onSubmit={handleOnSubmit}>
        <Paper elevation={12} className="form flex flex-col gap-4 mt-4 p-4">
          <div className="title font-bold text-center" data-testid="form-title">
            Sign in
          </div>

          <TextField
            label="Email Address"
            className="w-full"
            variant="filled"
            inputProps={{
              autoComplete: 'username',
              'data-testid': 'emailAddress',
            }}
            error={Boolean(formState.errors.emailAddress)}
            helperText={formState.errors.emailAddress?.message || undefined}
            {...register('emailAddress', validations.emailAddress)}
          />

          <TextField
            label="Password"
            type="password"
            className="w-full"
            variant="filled"
            inputProps={{
              autoComplete: 'current-password',
              'data-testid': 'password',
            }}
            error={Boolean(formState.errors.password)}
            helperText={formState.errors.password?.message || undefined}
            {...register('password', validations.password)}
          />

          <FormControlLabel
            htmlFor="rememberMe"
            label="Remember me"
            control={
              <Switch
                id="rememberMe"
                inputProps={
                  {
                    'data-testid': 'rememberMe',
                  } as InputHTMLAttributes<HTMLInputElement>
                }
                {...register('rememberMe', validations.rememberMe)}
              />
            }
          />

          <RecaptchaField
            className="flex justify-center"
            testid="recaptchaToken"
            control={control}
            name="recaptchaToken"
          />

          {submitLoading && (
            <LoadingIndicator delay={0} message="Signing in..." />
          )}

          {serverResult && !serverResult.isOk && (
            <Alert severity="error">
              {serverResult.error.errorMessages.map((x) => (
                <div data-testid="error-message" key={x}>
                  {x}
                </div>
              ))}
            </Alert>
          )}

          <Button
            type="submit"
            variant="contained"
            className="w-full"
            data-testid="submit-button"
            disabled={submitLoading}
          >
            Sign in
          </Button>
        </Paper>
      </form>
    </div>
  );
});
