import { createSlice, configureStore } from '@reduxjs/toolkit';

import { InMemoryStorage } from '~/test-utils';

import { ReduxPersistManager } from './ReduxPersistManager';

interface Slice1State {
  value: boolean;
}

const slice1InitialState: Slice1State = {
  value: false,
};

const slice1 = createSlice({
  name: 'slice1',
  initialState: slice1InitialState,
  reducers: {
    toggleState(state) {
      state.value = !state.value;
    },
  },
});

interface Slice2State {
  value: boolean;
}

const slice2InitialState: Slice2State = {
  value: false,
};

const slice2 = createSlice({
  name: 'slice2',
  initialState: slice2InitialState,
  reducers: {
    toggleState(state) {
      state.value = !state.value;
    },
  },
});

const createTestStore = (preloadedState?: unknown) =>
  configureStore({
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-explicit-any
    preloadedState: preloadedState as any,
    reducer: {
      [slice1.name]: slice1.reducer,
      [slice2.name]: slice2.reducer,
    },
  });

const STORAGE_KEY = 'STORAGE_KEY';

test('readPersistedState returns an empty object when storage location is empty', async () => {
  const reduxPersistManager = new ReduxPersistManager(
    new InMemoryStorage(),
    STORAGE_KEY,
    ['slice1']
  );

  expect(reduxPersistManager.readPersistedState()).toMatchInlineSnapshot(`{}`);
});

test('readPersistedState returns correct value when persisted state is available', async () => {
  const storage = new InMemoryStorage();

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  storage.setItem(STORAGE_KEY, JSON.stringify({ slice1: { value: true } }));

  expect(reduxPersistManager.readPersistedState()).toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": true,
      },
    }
  `);
});

test('readPersistedState returns and empty object when storage throws an error', async () => {
  const storage = new InMemoryStorage();
  storage.throwOnAccess = true;

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  expect(reduxPersistManager.readPersistedState()).toMatchInlineSnapshot(`{}`);
});

test('readPersistedState only returns fields that have been specified', async () => {
  const storage = new InMemoryStorage();

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  storage.setItem(
    STORAGE_KEY,
    JSON.stringify({
      slice1: { value: true },
      notSpecifiedKey: 'value',
    })
  );

  expect(reduxPersistManager.readPersistedState()).toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": true,
      },
    }
  `);
});

test('subscribe subscribes on state changes and saves the specified fields to given storage', async () => {
  const storage = new InMemoryStorage();

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  const store = createTestStore();

  reduxPersistManager.subscribe(store);

  expect(storage.getItem(STORAGE_KEY)).toBeUndefined();

  store.dispatch(slice1.actions.toggleState());

  expect(JSON.parse(storage.getItem(STORAGE_KEY) as string))
    .toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": true,
      },
    }
  `);

  store.dispatch(slice1.actions.toggleState());

  expect(JSON.parse(storage.getItem(STORAGE_KEY) as string))
    .toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": false,
      },
    }
  `);
});

test('storage is not updated when non persistent state is changed', async () => {
  const storage = new InMemoryStorage();
  storage.setItem(STORAGE_KEY, JSON.stringify({ slice1: { value: false } }));
  storage.setItem = jest.fn();

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  const persistedState = reduxPersistManager.readPersistedState();

  expect(persistedState).toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": false,
      },
    }
  `);

  const store = createTestStore(persistedState);

  reduxPersistManager.subscribe(store);

  store.dispatch(slice2.actions.toggleState());

  // eslint-disable-next-line @typescript-eslint/unbound-method
  expect(storage.setItem).not.toHaveBeenCalled();

  store.dispatch(slice2.actions.toggleState());

  // eslint-disable-next-line @typescript-eslint/unbound-method
  expect(storage.setItem).not.toHaveBeenCalled();
});

test('storage is not updated after unsubscribe is called', async () => {
  const storage = new InMemoryStorage();
  storage.setItem(STORAGE_KEY, JSON.stringify({ slice1: { value: false } }));

  const reduxPersistManager = new ReduxPersistManager(storage, STORAGE_KEY, [
    'slice1',
  ]);

  const persistedState = reduxPersistManager.readPersistedState();

  expect(persistedState).toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": false,
      },
    }
  `);

  const store = createTestStore(persistedState);

  const unsubscribe = reduxPersistManager.subscribe(store);

  store.dispatch(slice1.actions.toggleState());

  expect(JSON.parse(storage.getItem(STORAGE_KEY) as string))
    .toMatchInlineSnapshot(`
    {
      "slice1": {
        "value": true,
      },
    }
  `);

  storage.setItem = jest.fn();

  unsubscribe();

  store.dispatch(slice1.actions.toggleState());

  // eslint-disable-next-line @typescript-eslint/unbound-method
  expect(storage.setItem).not.toHaveBeenCalled();

  store.dispatch(slice1.actions.toggleState());

  // eslint-disable-next-line @typescript-eslint/unbound-method
  expect(storage.setItem).not.toHaveBeenCalled();
});
