import { screen, waitFor } from '@testing-library/react';

describe('<App />', () => {
  test('displays header', () => {
    // render(<App />);
    expect(screen.getByText('This is a header')).toBeInTheDocument();
  });

  test('displays names of cats', async () => {
    // render(<App />);
    await waitFor(() => {
      expect(screen.getByText('Sivtsu')).toBeInTheDocument();
    });

    await waitFor(() => {
      expect(screen.getByText('Kuzunak')).toBeInTheDocument();
    });
  });
});
