import { BaseRpcClient } from './BaseRpcClient';

// eslint-disable-next-line no-console
console.error = jest.fn();
// eslint-disable-next-line no-console
const consoleErrorMock = console.error;

const originalStringify = JSON.stringify;
JSON.stringify = jest.fn(originalStringify) as typeof JSON.stringify;
const stringifyMock = JSON.stringify as jest.Mock;

const originalParse = JSON.parse;
JSON.parse = jest.fn(originalParse) as typeof JSON.parse;
const parseMock = JSON.parse as jest.Mock;

window.fetch = jest.fn();
const fetchMock = window.fetch as jest.Mock;

describe('BaseRpcClient.sendResult', () => {
  afterEach(() => {
    jest.resetAllMocks();
    jest.resetModules();

    stringifyMock.mockImplementation(originalStringify);
    parseMock.mockImplementation(originalParse);
  });

  test('returns error result on falsy requestType', async () => {
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

  test('returns error result on serialization error', async () => {
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

  test('returns error result on fetch error', async () => {
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

  test('returns error result on non 200 status code', async () => {
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

  test('returns error result on failure to read the response', async () => {
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

  test('returns error result on failure to parse the response', async () => {
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
    await baseClient.sendResult('TestRequest', { testInput: 123 });

    expect(fetchMock).toHaveBeenCalledWith('/rpc/TestRequest', {
      body: '{"testInput":123}',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      method: 'POST',
    });
  });

  test('returns response body on successful response', async () => {
    fetchMock.mockImplementation(() =>
      Promise.resolve({
        status: 200,
        text: () => '{ "isOk": true, "payload": { "test": 123 } }',
      })
    );

    const baseClient = new BaseRpcClient();
    const result = await baseClient.sendResult('TestRequest');

    expect(result).toMatchInlineSnapshot(`
      Object {
        "isOk": true,
        "payload": Object {
          "test": 123,
        },
      }
    `);

    expect(consoleErrorMock).not.toHaveBeenCalled();
  });
});