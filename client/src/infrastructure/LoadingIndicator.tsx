import { memo, useState, useEffect } from 'react';
import { CircularProgress } from '@mui/material';

const defaultDelay = 500;

interface LoadingIndicatorProps {
  delay?: number;
  message?: string;
}

export const LoadingIndicator = memo(
  ({ message, delay }: LoadingIndicatorProps): JSX.Element | null => {
    const [showIndicator, setShowIndicator] = useState(delay === 0);

    useEffect(() => {
      if (delay === 0) {
        return;
      }

      const handle = setTimeout(() => {
        setShowIndicator(true);
      }, delay || defaultDelay);

      return () => {
        clearTimeout(handle);
      };
    }, [delay]);

    if (!showIndicator) {
      return null;
    }

    return (
      <div className="py-6">
        <div className="text-center">{message || 'Loading...'}</div>
        <div className="text-center mt-5 mr-3">
          <CircularProgress />
        </div>
      </div>
    );
  }
);
