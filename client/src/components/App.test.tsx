import { render, screen } from '@testing-library/react';

import { ResizeObserverMock, renderWithProviders } from '~test-utils';
import { createReduxStore } from '~infra/redux';

import { App, NotFoundPage } from './App';

beforeAll(() => {
  ResizeObserverMock.enableMock();
});

afterAll(() => {
  ResizeObserverMock.disableMock();
});

afterEach(() => {
  ResizeObserverMock.clearInstances();
  jest.resetAllMocks();
  jest.restoreAllMocks();
  jest.resetModules();
});

test('renders without error', async () => {
  const store = createReduxStore();

  render(<App store={store} />);

  const brandName = await screen.findByTestId('brand-name');

  expect(brandName).toHaveTextContent('Power Level');
});

test('renders correct message', async () => {
  renderWithProviders(<NotFoundPage />);

  expect(screen.getByText('Not found')).toBeInTheDocument();
});
