import { Result } from '~infrastructure/helpers';

const errors = {
  networkError: 'Network error.',
  serverError: 'An error occurred on the server.',
  falsyRequestType: 'Rpc client error: requestType is falsy.',
  serializeRequestError: 'Rpc client error: failed to serialize request body.',
  readRequestBodyError: 'Rpc client error: failed to read response body.',
  deserializeResponseError:
    'Rpc client error: failed to deserialize response body.',
};

const errorResult = <TResponse>(errorMessage: string): Result<TResponse> => ({
  isOk: false,
  errorMessages: [errorMessage],
});

const reportError = (message: string, error?: unknown): void => {
  if (!error) {
    // eslint-disable-next-line no-console
    console.error(error);
  } else {
    // eslint-disable-next-line no-console
    console.error(message, error);
  }
};

export class RpcClientHelper {
  async send<TRequest, TResponse = void>(
    requestType: string,
    requestBody?: TRequest
  ): Promise<Result<TResponse>> {
    if (!requestType) {
      reportError(errors.falsyRequestType);
      return errorResult(errors.falsyRequestType);
    } else {
      requestType = requestType.trim();
    }

    const request: RequestInit = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
    };

    try {
      request.body = JSON.stringify(requestBody || {});
    } catch (error) {
      reportError(errors.serializeRequestError, error);
      return errorResult(errors.serializeRequestError);
    }

    let response: Response;
    try {
      response = await fetch('/rpc/' + requestType, request);
    } catch (error) {
      reportError(errors.networkError, error);
      return errorResult(errors.networkError);
    }

    // TODO: Implement session rejection.

    // TODO: implement server versioning.

    if (response.status !== 200) {
      return errorResult(errors.serverError);
    }

    let json: string;
    try {
      json = await response.text();
    } catch (error) {
      reportError(errors.readRequestBodyError, error);
      return errorResult(errors.readRequestBodyError);
    }

    let responseBody: Result<TResponse>;
    try {
      responseBody = JSON.parse(json) as Result<TResponse>;
    } catch (error) {
      reportError(errors.deserializeResponseError, error);
      return errorResult(errors.deserializeResponseError);
    }

    return responseBody;
  }
}
