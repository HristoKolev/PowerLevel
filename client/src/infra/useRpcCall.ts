import { useCallback, useState, useEffect } from 'react';

import { RpcClient } from '~infra/RpcClient';
import { useRpcClient } from '~infra/useRpcClient';

export const useRpcCall = function useRpcCall<T>(
  fn: (client: RpcClient) => Promise<T>,
  deps: unknown[]
) {
  const rpcClient = useRpcClient();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const fnCallback = useCallback(fn, deps);
  const [serverResult, setServerResult] = useState<T | undefined>();

  useEffect(() => {
    void (async () => {
      setServerResult(undefined);
      const result = await fnCallback(rpcClient);
      setServerResult(result);
      // eslint-disable-next-line no-console
    })().catch(console.error); // TODO: Add error reporting here
  }, [fnCallback, rpcClient]);

  return serverResult;
};
