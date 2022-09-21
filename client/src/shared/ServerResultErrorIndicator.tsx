import { memo } from 'react';
import { Alert } from '@mui/material';

import { ApiResult } from '~infra/api-result';

// TODO: extract a separate ErrorIndicator component and bease this one on that.
interface ServerResultErrorIndicatorProps {
  // TODO: allow multiple results.
  result: ApiResult<unknown> | undefined;
  testId?: string | undefined;
}

export const ServerResultErrorIndicator = memo(
  ({
    result,
    testId = 'error-message',
  }: ServerResultErrorIndicatorProps): JSX.Element | null => {
    if (!result || result.isOk) {
      return null;
    }

    return (
      <Alert severity="error">
        {result.error.errorMessages.map((x) => (
          <div data-testid={testId} key={x}>
            {x}
          </div>
        ))}
      </Alert>
    );
  }
);
