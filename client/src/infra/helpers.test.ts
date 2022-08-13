import { delay } from './helpers';

jest.spyOn(global, 'setTimeout');

afterEach(() => {
  jest.resetAllMocks();
  jest.restoreAllMocks();
  jest.resetModules();
});

test('delay resolves after the timeout expires', async () => {
  await delay(10);
  expect(setTimeout).toHaveBeenCalledWith(expect.any(Function), 10);
});
