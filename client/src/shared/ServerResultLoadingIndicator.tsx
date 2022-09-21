import { memo } from 'react';

import { ApiResult } from '~infra/api-result';
import {
  LoadingIndicator,
  LoadingIndicatorProps,
} from '~shared/LoadingIndicator';

interface ServerResultLoadingIndicatorProps extends LoadingIndicatorProps {
  // TODO: allow multiple results.
  result: ApiResult<unknown> | undefined;
}

export const ServerResultLoadingIndicator = memo(
  ({
    result,
    ...rest
  }: ServerResultLoadingIndicatorProps): JSX.Element | null => {
    if (!result) {
      return <LoadingIndicator {...rest} />;
    }

    return null;
  }
);
