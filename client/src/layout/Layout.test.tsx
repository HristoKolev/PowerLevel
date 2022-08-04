import { screen, waitFor, act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import {
  ResizeObserverMock,
  renderWithProviders,
} from '~infrastructure/test-utils';
import { createStore } from '~infrastructure/redux';
import { ResponsiveDrawerBreakpoint } from '~layout/layoutSlice';

import { Layout } from './Layout';

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

test('renders children', async () => {
  const testID = 'layout-content';

  renderWithProviders(
    <Layout>
      <div data-testid={testID} />
    </Layout>
  );

  expect(screen.getByTestId(testID)).toBeInTheDocument();
});

test('app bar icon toggles the drawer', async () => {
  renderWithProviders(
    <Layout>
      <div />
    </Layout>
  );

  const appBarDrawerToggle = await screen.findByTestId('app-bar-drawer-toggle');

  const drawerMenuTitle = await screen.findByTestId('drawer-menu-title');

  await waitFor(() => {
    expect(drawerMenuTitle).not.toBeVisible();
  });

  await userEvent.click(appBarDrawerToggle);

  await waitFor(() => {
    expect(drawerMenuTitle).toBeVisible();
  });
});

test('drawer icon toggles the drawer', async () => {
  renderWithProviders(
    <Layout>
      <div />
    </Layout>
  );

  const appBarDrawerToggle = await screen.findByTestId('app-bar-drawer-toggle');

  const drawerMenuTitle = await screen.findByTestId('drawer-menu-title');

  await waitFor(() => {
    expect(drawerMenuTitle).not.toBeVisible();
  });

  await userEvent.click(appBarDrawerToggle);

  await waitFor(() => {
    expect(drawerMenuTitle).toBeVisible();
  });

  const drawerToggle = await screen.findByTestId('drawer-toggle');

  await userEvent.click(drawerToggle);

  await waitFor(() => {
    expect(drawerMenuTitle).not.toBeVisible();
  });
});

test("breakpoint changes when the root element's size changes", async () => {
  const store = createStore();

  renderWithProviders(
    <Layout>
      <div />
    </Layout>,
    store
  );

  expect(store.getState().LAYOUT.breakpoint).toEqual(
    ResponsiveDrawerBreakpoint.Overlaid
  );

  act(() => {
    const instance = ResizeObserverMock.getSingleInstance();
    instance.observerCallback([], instance);
    instance.observerCallback(
      [{ contentRect: { width: 1000 } } as ResizeObserverEntry],
      instance
    );
  });

  await waitFor(() => {
    expect(store.getState().LAYOUT.breakpoint).toEqual(
      ResponsiveDrawerBreakpoint.SideBySide
    );
  });

  act(() => {
    const instance = ResizeObserverMock.getSingleInstance();
    instance.observerCallback([], instance);
  });

  await waitFor(() => {
    expect(store.getState().LAYOUT.breakpoint).toEqual(
      ResponsiveDrawerBreakpoint.SideBySide
    );
  });

  act(() => {
    const instance = ResizeObserverMock.getSingleInstance();
    instance.observerCallback([], instance);
    instance.observerCallback(
      [{ contentRect: { width: 800 } } as ResizeObserverEntry],
      instance
    );
  });

  await waitFor(() => {
    expect(store.getState().LAYOUT.breakpoint).toEqual(
      ResponsiveDrawerBreakpoint.Overlaid
    );
  });
});
