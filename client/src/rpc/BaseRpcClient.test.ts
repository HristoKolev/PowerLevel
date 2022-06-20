import { BaseRpcClient, RpcError, ApiResult } from './BaseRpcClient';

const consoleErrorMock = jest.fn();
Reflect.set(console, 'error', consoleErrorMock);

const originalStringify = JSON.stringify;
const stringifyMock = jest.fn(originalStringify);
JSON.stringify = stringifyMock as typeof JSON.stringify;

const originalParse = JSON.parse;
const parseMock = jest.fn(originalParse);
JSON.parse = parseMock as typeof JSON.parse;

const fetchMock = jest.fn();
window.fetch = fetchMock;

describe('BaseRpcClient', () => {
  afterEach(() => {
    jest.resetAllMocks();
    jest.restoreAllMocks();
    jest.resetModules();

    stringifyMock.mockImplementation(originalStringify);
    parseMock.mockImplementation(originalParse);
  });

  test('sendResult returns error result on falsy requestType', async () => {
    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult(null as unknown as string);
    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "Rpc client error: requestType is falsy.",
          ],
        },
        "isOk": false,
      }
    `);
    expect(consoleErrorMock).toHaveBeenCalled();
  });

  test('sendResult returns error result on serialization error', async () => {
    stringifyMock.mockImplementation(() => {
      throw new Error('Serialization failed.');
    });

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "Rpc client error: failed to serialize request body.",
          ],
        },
        "isOk": false,
      }
    `);

    expect(consoleErrorMock).toHaveBeenCalled();
  });

  test('sendResult returns error result on fetch error', async () => {
    fetchMock.mockImplementation(() => {
      throw new Error('Fetch failed.');
    });

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "Network error.",
          ],
        },
        "isOk": false,
      }
    `);

    expect(consoleErrorMock).toHaveBeenCalled();
  });

  test('sendResult returns error result on non 200 status code', async () => {
    fetchMock.mockImplementation(() => Promise.resolve({ status: 500 }));

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "An error occurred on the server.",
          ],
        },
        "isOk": false,
      }
    `);
  });

  test('sendResult returns error result on failure to read the response', async () => {
    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => {
          throw new Error('response.text() failed.');
        },
      })
    );

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "Rpc client error: failed to read response body.",
          ],
        },
        "isOk": false,
      }
    `);

    expect(consoleErrorMock).toHaveBeenCalled();
  });

  test('sendResult returns error result on failure to parse the response', async () => {
    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => '{}',
      })
    );

    parseMock.mockImplementation(() => {
      throw new Error('Error parsing response body.');
    });

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "error": Object {
          "errorMessages": Array [
            "Rpc client error: failed to deserialize response body.",
          ],
        },
        "isOk": false,
      }
    `);

    expect(consoleErrorMock).toHaveBeenCalled();
  });

  test('window.fetch is called with the correct parameters', async () => {
    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => '{ "isOk": true, "payload": { "test": 123 } }',
      })
    );

    const baseClient = new BaseRpcClient();

    const requestBody = { testInput: 123 };

    await baseClient.sendResult('TestRequest', requestBody);

    expect(fetchMock).toHaveBeenCalledWith('/rpc/TestRequest', {
      body: JSON.stringify(requestBody),
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      method: 'POST',
    });
  });

  test('sendResult returns response body on successful response', async () => {
    const mockResult: ApiResult<{ test: number }> = {
      isOk: true,
      payload: { test: 123 },
    };

    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => JSON.stringify(mockResult),
      })
    );

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toEqual(mockResult);

    expect(consoleErrorMock).not.toHaveBeenCalled();
  });

  test('CSRF token is sent to the server', async () => {
    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => '{ "isOk": true, "payload": { "test": 123 } }',
      })
    );

    const csrfToken = '[Token]';

    const baseClient = new BaseRpcClient();

    baseClient.setCSRFToken(csrfToken);

    await baseClient.sendResult('TestRequest');

    expect(fetchMock).toHaveBeenCalledWith('/rpc/TestRequest', {
      body: '{}',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'Csrf-Token': csrfToken,
      },
      method: 'POST',
    });

    fetchMock.mockClear();

    baseClient.clearCSRFToken();

    await baseClient.sendResult('TestRequest');

    expect(fetchMock).toHaveBeenCalledWith('/rpc/TestRequest', {
      body: '{}',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      method: 'POST',
    });
  });

  test('send returns payload on success', async () => {
    const result: ApiResult<{ test: number }> = {
      isOk: true,
      payload: { test: 123 },
    };

    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => JSON.stringify(result),
      })
    );

    const baseClient = new BaseRpcClient();

    const payload = await baseClient.send('TestRequest');

    expect(payload).toEqual(result.payload);
  });

  test('throws RpcError on failure', async () => {
    const result: ApiResult = {
      isOk: false,
      error: {
        errorID: '123',
        errorMessages: [
          'This is the first error message',
          'This is the second error message',
        ],
      },
    };

    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => JSON.stringify(result),
      })
    );

    const baseClient = new BaseRpcClient();

    try {
      await baseClient.send('TestRequest');
      throw new Error('baseClient.send should have thrown an error.');
    } catch (error) {
      const rpcError = error as RpcError;

      expect(rpcError.errorID).toEqual(result.error.errorID);
      expect(rpcError.message).toEqual(result.error.errorMessages.join(', '));
    }
  });
});
