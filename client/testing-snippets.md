
### RecaptchaField

```tsx

// eslint-disable-next-line @typescript-eslint/no-unsafe-return
jest.mock('~shared/RecaptchaField', () => ({
  ...jest.requireActual('~shared/RecaptchaField'),
  RecaptchaField: ({ testid }: { testid: string }) => (
    <div data-testid={testid} />
  ),
}));

```

### RpcClient

```tsx

type RpcClientMockType = { [key in keyof RpcClient]: jest.Mock };

jest.mock('~infra/RpcClient', () => {
  const MockedRpcClient: RpcClientMockType = (() => {
    const map = new Map<string, jest.Mock>();
    const proxy: RpcClientMockType = new Proxy(
      function () {} as unknown as RpcClientMockType,
      {
        construct() {
          return proxy;
        },
        get(_, key: string): jest.Mock {
          if (!map.has(key)) {
            map.set(key, jest.fn());
          }
          return map.get(key) as jest.Mock;
        },
      }
    );
    return proxy;
  })();
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('~infra/RpcClient'),
    RpcClient: MockedRpcClient,
  };
});

const RpcClientMock = RpcClient as unknown as RpcClientMockType;


```

### React Router useNavigate

```tsx

jest.mock('react-router-dom', () => {
  const navigate = jest.fn();
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  return {
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => navigate,
  };
});

// eslint-disable-next-line react-hooks/rules-of-hooks
const navigateMock = useNavigate() as jest.Mock;


```


### ResizeObserverMock

* At the top of the test suite

```tsx

beforeAll(() => {
  ResizeObserverMock.enableMock();
});

afterAll(() => {
  ResizeObserverMock.disableMock();
});

afterEach(() => {
  ResizeObserverMock.clearInstances();
});

```

* In the test:

```tsx

act(() => {
  const instance = ResizeObserverMock.getSingleInstance();
  
  instance.observerCallback(
    [{ contentRect: { width: 1000 } } as ResizeObserverEntry],
    instance
  );
});


```
