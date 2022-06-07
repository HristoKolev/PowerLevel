export type Result<T = null> =
  | { isOk: true; payload: T }
  | { isOk: false; errorMessages: string[] };

export interface SelectItem<T = unknown> {
  value: T;
  label: string;
}

export const delay = (interval: number): Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, interval));
