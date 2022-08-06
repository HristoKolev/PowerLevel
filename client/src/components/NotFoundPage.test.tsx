import { screen } from '@testing-library/react';

import { renderWithProviders } from '~infrastructure/test-utils';

import { NotFoundPage } from './NotFoundPage';

afterEach(() => {
  jest.resetAllMocks();
  jest.restoreAllMocks();
  jest.resetModules();
});

test('renders correct message', async () => {
  renderWithProviders(<NotFoundPage />);

  expect(screen.getByText('Not found')).toBeInTheDocument();
});
