import { RpcClient } from '~infra/RpcClient';

type RpcClientMockType = { [key in keyof RpcClient]: jest.Mock };

const MockedRpcClient: RpcClientMockType = (() => {
  const map = new Map<string, jest.Mock>();
  const proxy: RpcClientMockType = new Proxy(
    function () {} as unknown as RpcClientMockType,
    {
      construct() {
        return proxy;
      },
      get(_, key: string): jest.Mock {
        if (!map.has(key)) {
          map.set(key, jest.fn());
        }
        return map.get(key) as jest.Mock;
      },
    }
  );

  return proxy;
})();
// eslint-disable-next-line @typescript-eslint/no-unsafe-return

// eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
module.exports = {
  ...jest.requireActual('~infra/RpcClient'),
  RpcClient: MockedRpcClient,
};
