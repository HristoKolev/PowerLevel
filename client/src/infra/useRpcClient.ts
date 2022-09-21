import { useSelector, useDispatch } from 'react-redux';
import { useMemo } from 'react';

import { csrfTokenSelector } from '~auth/sessionSlice';

import { createRpcClient } from './create-rpc-client';

export const useRpcClient = () => {
  const csrfToken = useSelector(csrfTokenSelector);
  const dispatch = useDispatch();
  return useMemo(
    () => createRpcClient(csrfToken, dispatch),
    [csrfToken, dispatch]
  );
};
