import { ReactElement } from 'react';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';
// eslint-disable-next-line import/no-extraneous-dependencies
import { RenderOptions, RenderResult, render } from '@testing-library/react';

import { ReduxStoreType, createReduxStore } from '~infra/redux';

export function renderWithProviders(
  ui: ReactElement,
  store?: ReduxStoreType,
  options?: Omit<RenderOptions, 'queries'>
): RenderResult {
  return render(
    <BrowserRouter>
      <Provider store={store || createReduxStore()}>{ui}</Provider>
    </BrowserRouter>,
    options
  );
}

export class ResizeObserverMock {
  private static originalResizeObserver: unknown;

  static enableMock() {
    ResizeObserverMock.clearInstances();

    if (!ResizeObserverMock.originalResizeObserver) {
      ResizeObserverMock.originalResizeObserver = Reflect.get(
        window,
        'ResizeObserver'
      );
    }

    Reflect.set(window, 'ResizeObserver', ResizeObserverMock);
  }

  static disableMock() {
    ResizeObserverMock.clearInstances();

    Reflect.set(
      window,
      'ResizeObserver',
      ResizeObserverMock.originalResizeObserver
    );
  }

  private static instances: ResizeObserverMock[] = [];

  static getInstances(): ResizeObserverMock[] {
    return [...ResizeObserverMock.instances];
  }

  static getSingleInstance(): ResizeObserverMock {
    const instances = ResizeObserverMock.getInstances();

    if (!instances.length) {
      throw new Error('No instance of ResizeObserverMock was registered.');
    }

    if (instances.length > 1) {
      throw new Error(
        'More than one instance of ResizeObserverMock was registered.'
      );
    }

    return instances[0];
  }

  static clearInstances() {
    ResizeObserverMock.instances = [];
  }

  disconnectMock: jest.Mock;

  observeMock: jest.Mock;

  unobserveMock: jest.Mock;

  observerCallback: ResizeObserverCallback;

  constructor(observerCallback: ResizeObserverCallback) {
    this.disconnectMock = jest.fn();
    this.observeMock = jest.fn();
    this.unobserveMock = jest.fn();

    this.observerCallback = observerCallback;

    ResizeObserverMock.instances.push(this);
  }

  disconnect(): void {
    this.disconnectMock.call(this);
  }

  observe(target: Element, options?: ResizeObserverOptions): void {
    this.observeMock.call(this, target, options);
  }

  unobserve(target: Element): void {
    this.unobserveMock.call(this, target);
  }
}

export class InMemoryStorage implements Storage {
  private readonly backingObject: Record<string, string | null>;

  throwOnAccess = false;

  constructor(backingObject?: Record<string, string | null>) {
    this.backingObject = backingObject || {};
  }

  get(key: string): string | null {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    return this.backingObject[key];
  }

  set(key: string, value: string | null) {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    this.backingObject[key] = value;
  }

  get length(): number {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    return Object.keys(this.backingObject).length;
  }

  clear(): void {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    for (const key of Reflect.ownKeys(this.backingObject)) {
      delete this.backingObject[key as string];
    }
  }

  getItem(key: string): string | null {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    return this.backingObject[key];
  }

  key(index: number): string | null {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    return (
      Object.keys(this.backingObject)
        .sort((x, y) => x.localeCompare(y))
        .find((_, i) => i === index) || null
    );
  }

  removeItem(key: string): void {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    delete this.backingObject[key];
  }

  setItem(key: string, value: string): void {
    if (this.throwOnAccess) {
      throw new Error('InMemoryStorage access error.');
    }

    this.backingObject[key] = value;
  }
}

export class WaitHandle {
  private resolveCollection: (() => void)[] = [];

  release() {
    for (const resolve of this.resolveCollection) {
      resolve();
    }
    this.resolveCollection = [];
  }

  wait(): Promise<void> {
    return new Promise((resolve) => {
      this.resolveCollection.push(resolve);
    });
  }
}
