import { DefaultApiError } from './RpcClient';

export type ApiResult<
  TPayload = null,
  TError extends DefaultApiError = DefaultApiError
> = { isOk: true; payload: TPayload } | { isOk: false; error: TError };

export const apiResult = {
  ok: <TPayload>(payload: TPayload): ApiResult<TPayload> => ({
    payload,
    isOk: true,
  }),
  error: <TError extends DefaultApiError = DefaultApiError>(error: TError) => ({
    isOk: false,
    error,
  }),
};
