import { screen, waitFor } from '@testing-library/react';

import { renderWithProviders } from '~infrastructure/test-utils';
import { LoadingIndicator } from '~infrastructure/LoadingIndicator';

test('shows ui immediately if the delay is 0', async () => {
  renderWithProviders(<LoadingIndicator delay={0} />);

  expect(screen.getByTestId('loading-indicator')).toBeInTheDocument();
});

test('shows message', async () => {
  const message = 'Test message';

  renderWithProviders(<LoadingIndicator delay={0} message={message} />);

  expect(screen.getByTestId('loading-indicator')).toHaveTextContent(message);
});

test('does not show indicator until the delay has passed', async () => {
  renderWithProviders(<LoadingIndicator delay={10} />);

  expect(screen.queryByTestId('loading-indicator')).not.toBeInTheDocument();

  await waitFor(() => {
    expect(screen.getByTestId('loading-indicator')).toBeInTheDocument();
  });
});
