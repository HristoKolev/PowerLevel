import { memo, useState, useCallback, ChangeEvent } from 'react';
import { css } from '@linaria/core';
import {
  Button,
  Paper,
  TextField,
  Switch,
  FormControlLabel,
  Alert,
} from '@mui/material';

import {
  BaseRpcClient,
  RpcClient,
  ApiResult,
  LoginResponse,
  LoginError,
} from '~rpc';

const signInPageClassName = css`
  .form {
    width: 400px;
  }

  .title {
    font-size: 1.4rem;
  }
`;

export const SignInPage = memo((): JSX.Element => {
  const [emailTouched, setEmailTouched] = useState(false);
  const [emailAddress, setEmailAddress] = useState<string>('');
  const handleOnEmailAddressChange = useCallback(
    (ev: ChangeEvent<HTMLInputElement>) => {
      setEmailTouched(true);
      setEmailAddress(ev.target.value);
    },
    []
  );
  const handleOnEmailAddressBlur = useCallback(() => {
    setEmailTouched(true);
  }, []);

  const [passwordTouched, setPasswordTouched] = useState(false);
  const [password, setPassword] = useState<string>('');
  const handleOnPasswordChange = useCallback(
    (ev: ChangeEvent<HTMLInputElement>) => {
      setPasswordTouched(true);
      setPassword(ev.target.value);
    },
    []
  );
  const handleOnPasswordBlur = useCallback(() => {
    setPasswordTouched(true);
  }, []);

  const [rememberMe, setRememberMe] = useState<boolean>(false);
  const handleOnRememberChange = useCallback(
    (ev: ChangeEvent<HTMLInputElement>) => {
      setRememberMe(ev.target.checked);
    },
    []
  );

  const [serverResult, setServerResult] = useState<
    ApiResult<LoginResponse, LoginError> | undefined
  >();
  const handleOnSubmit = useCallback(async () => {
    setEmailTouched(true);
    setPasswordTouched(true);

    if (!emailAddress || !password) {
      return;
    }

    const rpcClient = new RpcClient(new BaseRpcClient());
    const result = await rpcClient.loginResult({
      emailAddress,
      password,
      rememberMe,
    });

    setServerResult(result);
  }, [password, rememberMe, emailAddress]);

  return (
    <div className={`flex justify-around ${signInPageClassName}`}>
      <Paper elevation={12} className="form flex flex-col gap-4 mt-4 p-4">
        <div className="title font-bold text-center">Sign in</div>

        <TextField
          id="emailAddress"
          label="Email Address"
          required
          className="w-full"
          value={emailAddress}
          onChange={handleOnEmailAddressChange}
          error={emailTouched && !emailAddress}
          onBlur={handleOnEmailAddressBlur}
          helperText={
            !emailAddress && emailTouched
              ? 'The email address field is required.'
              : undefined
          }
        />

        <TextField
          id="password"
          label="Password"
          type="password"
          required
          className="w-full"
          value={password}
          onChange={handleOnPasswordChange}
          error={passwordTouched && !password}
          onBlur={handleOnPasswordBlur}
          helperText={
            !password && passwordTouched
              ? 'The password field is required.'
              : undefined
          }
        />

        <FormControlLabel
          control={
            <Switch value={rememberMe} onChange={handleOnRememberChange} />
          }
          label="Remember me"
        />

        {serverResult && !serverResult.isOk && (
          <Alert severity="error">
            {serverResult.error.errorMessages.map((x) => (
              <div>{x}</div>
            ))}
          </Alert>
        )}

        {/* eslint-disable-next-line @typescript-eslint/no-misused-promises */}
        <Button variant="contained" className="w-full" onClick={handleOnSubmit}>
          Sign in
        </Button>
      </Paper>
    </div>
  );
});
