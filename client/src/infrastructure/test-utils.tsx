import { ReactNode } from 'react';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';

import { StoreType, createStore } from '~infrastructure/redux';

interface TestSetupProps {
  children: ReactNode;
  store?: StoreType;
}

export const TestSetup = ({ children, store }: TestSetupProps) => (
  <BrowserRouter>
    <Provider store={store || createStore()}>{children}</Provider>
  </BrowserRouter>
);

export class ResizeObserverMock {
  private static originalResizeObserver: unknown;

  static enableMock() {
    ResizeObserverMock.clearInstances();

    if (!ResizeObserverMock.originalResizeObserver) {
      ResizeObserverMock.originalResizeObserver = Reflect.get(
        global,
        'ResizeObserver'
      );
    }

    Reflect.set(global, 'ResizeObserver', ResizeObserverMock);
  }

  static disableMock() {
    ResizeObserverMock.clearInstances();

    Reflect.set(
      global,
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
