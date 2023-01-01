import { delay } from '~infra/helpers';

jest.useFakeTimers();

test('delay resolves after the timeout expires', async () => {
  const promise = delay(10);

  jest.advanceTimersByTime(10);

  await promise;
});
