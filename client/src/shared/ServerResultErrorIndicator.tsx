import { memo } from 'react';
import { Alert } from '@mui/material';
import { css } from '@linaria/core';

import { ApiResult } from '~infra/api-result';

// TODO: extract a separate ErrorIndicator component and bease this one on that.
interface ServerResultErrorIndicatorProps {
  // TODO: allow multiple results.
  result: ApiResult<unknown> | undefined;
  testId?: string | undefined;
}

const wrapperClassName = css`
  background-color: rgb(253, 237, 237);
  border-radius: 4px;
`;

export const ServerResultErrorIndicator = memo(
  ({
    result,
    testId = 'error-message',
  }: ServerResultErrorIndicatorProps): JSX.Element | null => {
    if (!result || result.isOk) {
      return null;
    }

    return (
      <div
        className={`${wrapperClassName} w-full h-full flex items-center justify-center`}
      >
        <Alert severity="error">
          {result.error.errorMessages.map((x) => (
            <div data-testid={testId} key={x}>
              {x}
            </div>
          ))}
        </Alert>
      </div>
    );
  }
);
