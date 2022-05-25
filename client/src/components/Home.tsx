import { Fragment, memo } from 'react';
import { Alert, AlertTitle, Button } from '@mui/material';

export const Home = memo(
  (): JSX.Element => (
    <div>
      {Array(100)
        .fill(undefined)
        .map((_, i) => (
          <Fragment key={i}>
            <Button variant="contained">Contained</Button>
            <Alert severity="error">
              <AlertTitle>Error</AlertTitle>
              This is an error alert — <strong>check it out!</strong>
            </Alert>
          </Fragment>
        ))}
    </div>
  )
);
