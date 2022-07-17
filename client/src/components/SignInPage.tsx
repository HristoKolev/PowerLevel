import { memo, useState, useCallback, useMemo } from 'react';
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

import {
  ApiResult,
  LoginResponse,
  LoginError,
  BaseRpcClient,
  RpcClient,
  LoginRequest,
  rpcValidations,
} from '~rpc';

const signInPageClassName = css`
  .form {
    width: 400px;
  }

  .title {
    font-size: 1.4rem;
  }
`;

const validations = rpcValidations.loginRequest;

export const SignInPage = memo((): JSX.Element => {
  const [serverResult, setServerResult] = useState<
    ApiResult<LoginResponse, LoginError> | undefined
  >();

  const onSubmit = useCallback(async (formValues: LoginRequest) => {
    const rpcClient = new RpcClient(new BaseRpcClient());
    const result = await rpcClient.loginResult(formValues);
    setServerResult(result);
  }, []);

  const { register, handleSubmit, formState } = useForm<LoginRequest>({
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
          <div className="title font-bold text-center">Sign in</div>

          <TextField
            label="Email Address"
            id="emailAddress"
            className="w-full"
            variant="filled"
            inputProps={{ autoComplete: 'username' }}
            error={Boolean(formState.errors.emailAddress)}
            helperText={formState.errors.emailAddress?.message || undefined}
            {...register('emailAddress', validations.emailAddress)}
          />

          <TextField
            label="Password"
            type="password"
            className="w-full"
            variant="filled"
            inputProps={{ autoComplete: 'current-password' }}
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
                {...register('rememberMe', validations.rememberMe)}
              />
            }
          />

          {serverResult && !serverResult.isOk && (
            <Alert severity="error">
              {serverResult.error.errorMessages.map((x) => (
                <div key={x}>{x}</div>
              ))}
            </Alert>
          )}

          <Button type="submit" variant="contained" className="w-full">
            Sign in
          </Button>
        </Paper>
      </form>
    </div>
  );
});
