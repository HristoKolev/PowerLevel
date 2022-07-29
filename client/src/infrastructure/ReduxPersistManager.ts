import { Store } from 'redux';

export class ReduxPersistManager {
  private readonly storage: Storage;

  private readonly storageKey: string;

  private readonly fieldNames: string[];

  private cachedState: Record<string, unknown> = {};

  constructor(storage: Storage, storageKey: string, fieldNames: string[]) {
    this.fieldNames = Array.from(new Set(fieldNames));
    this.storage = storage;
    this.storageKey = storageKey;
  }

  subscribe<TStore extends Store>(store: TStore): () => void {
    return store.subscribe(() => {
      const state = store.getState() as Record<string, unknown>;

      let changed = false;

      for (const fieldName of this.fieldNames) {
        if (this.cachedState[fieldName] !== state[fieldName]) {
          this.cachedState[fieldName] = state[fieldName];
          changed = true;
        }
      }

      if (!changed) {
        return;
      }

      const json = JSON.stringify(this.cachedState);

      try {
        this.storage.setItem(this.storageKey, json);
      } catch (error) {
        // ignore
        // TODO: Log error.
      }
    });
  }

  readPersistedState(): Record<string, unknown> {
    let json: string | null;

    try {
      json = this.storage.getItem(this.storageKey);
    } catch (error) {
      // TODO: Log error.
      return {};
    }

    let persistedState: Record<string, unknown>;
    if (json) {
      persistedState = JSON.parse(json) as Record<string, unknown>;
    } else {
      persistedState = {};
    }

    // Copy only the relevant fields.
    for (const fieldName of this.fieldNames) {
      const fieldValue = persistedState[fieldName];
      if (fieldValue !== undefined) {
        this.cachedState[fieldName] = fieldValue;
      }
    }

    return Object.assign({}, this.cachedState);
  }
}
