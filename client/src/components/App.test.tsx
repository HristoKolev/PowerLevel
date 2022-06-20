import { render, screen } from '@testing-library/react';

import { App } from '~components/App';
import { createStore } from '~infrastructure/redux';
import { ResizeObserverMock } from '~infrastructure/test-utils';

describe('<App />', () => {
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

  test('renders correctly', async () => {
    const store = createStore();

    render(<App store={store} />);

    const brandName = await screen.findByTestId('brand-name');

    expect(brandName).toBeInTheDocument();

    expect(brandName).toHaveTextContent('Power Level');
  });
});
