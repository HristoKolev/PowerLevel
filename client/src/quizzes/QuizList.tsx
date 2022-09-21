import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
} from '@mui/material';
import { css } from '@linaria/core';

import { useRpcCall } from '~infra/useRpcCall';
import { ServerResultErrorIndicator } from '~shared/ServerResultErrorIndicator';
import { ServerResultLoadingIndicator } from '~shared/ServerResultLoadingIndicator';

const quizListClassName = css`
  .card {
    width: 1000px;
  }
`;

export const QuizList = (): JSX.Element => {
  const serverResult = useRpcCall(
    (x) => x.searchQuizzesResult({ query: '' }),
    []
  );

  return (
    <div className={`${quizListClassName} flex justify-around`}>
      <Paper elevation={12} className="card flex flex-col gap-4 mt-4 p-4">
        <div className="title font-bold text-center" data-testid="card-title">
          Quizzes
        </div>

        <ServerResultLoadingIndicator result={serverResult} />
        <ServerResultErrorIndicator result={serverResult} />
        {serverResult?.isOk && (
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>#</TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell>Edit</TableCell>
                  <TableCell>Delete</TableCell>
                  <TableCell>Play</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {serverResult.payload.items.map((x) => (
                  <TableRow key={x.quizID}>
                    <TableCell>{x.quizID}</TableCell>
                    <TableCell>{x.quizName}</TableCell>
                    <TableCell>
                      <Button size="small" variant="contained" color="warning">
                        Edit
                      </Button>
                    </TableCell>
                    <TableCell>
                      <Button size="small" variant="contained" color="error">
                        Delete
                      </Button>
                    </TableCell>
                    <TableCell>
                      <Button size="small" variant="contained" color="success">
                        Play
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Paper>
    </div>
  );
};
