import { memo, useState, useEffect } from 'react';
import { CircularProgress } from '@mui/material';

export interface LoadingIndicatorProps {
  delay?: number;
  message?: string;
  testId?: string;
}

export const LoadingIndicator = memo(
  ({
    delay = 500,
    message = 'Loading...',
    testId = 'loading-indicator',
  }: LoadingIndicatorProps): JSX.Element | null => {
    const [showIndicator, setShowIndicator] = useState(delay === 0);

    useEffect(() => {
      if (delay === 0) {
        return;
      }

      const handle = setTimeout(() => setShowIndicator(true), delay);
      return () => clearTimeout(handle);
    }, [delay]);

    if (!showIndicator) {
      return null;
    }

    return (
      <div className="py-6" data-testid={testId}>
        <div className="text-center">{message}</div>
        <div className="text-center mt-5 mr-3">
          <CircularProgress />
        </div>
      </div>
    );
  }
);
