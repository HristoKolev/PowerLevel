import { memo, useState, useCallback } from 'react';
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
  ApiResult,
  LoginResponse,
  LoginError,
  BaseRpcClient,
  RpcClient,
  LoginRequest,
  rpcValidations,
} from '~rpc';
import { CustomForm, CustomField } from '~infrastructure/CustomForm';

const signInPageClassName = css`
  .form {
    width: 400px;
  }

  .title {
    font-size: 1.4rem;
  }
`;

export const SignInPage = memo((): JSX.Element => {
  const [serverResult, setServerResult] = useState<
    ApiResult<LoginResponse, LoginError> | undefined
  >();

  const handleOnSubmit = useCallback(async (formValues: LoginRequest) => {
    const rpcClient = new RpcClient(new BaseRpcClient());
    const result = await rpcClient.loginResult(formValues);
    setServerResult(result);
  }, []);

  return (
    <div className={`flex justify-around ${signInPageClassName}`}>
      <CustomForm
        validations={rpcValidations.loginRequest}
        onSubmit={handleOnSubmit}
      >
        <Paper elevation={12} className="form flex flex-col gap-4 mt-4 p-4">
          <div className="title font-bold text-center">Sign in</div>

          <CustomField
            id="emailAddress"
            render={(props) => (
              <TextField
                label="Email Address"
                className="w-full"
                variant="filled"
                inputProps={{ autoComplete: 'username' }}
                {...props}
              />
            )}
          />

          <CustomField
            id="password"
            render={(props) => (
              <TextField
                label="Password"
                type="password"
                className="w-full"
                variant="filled"
                inputProps={{ autoComplete: 'current-password' }}
                {...props}
              />
            )}
          />

          <CustomField
            id="rememberMe"
            render={(props) => (
              <FormControlLabel
                htmlFor={props.id}
                label="Remember me"
                control={
                  <Switch
                    id={props.id}
                    name={props.name}
                    value={props.value}
                    onChange={props.onChange}
                  />
                }
                onBlur={props.onBlur}
              />
            )}
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
      </CustomForm>
    </div>
  );
});
