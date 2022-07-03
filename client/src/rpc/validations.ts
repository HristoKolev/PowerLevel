export const validations = {
  required:
    (message: string) =>
    (value: unknown): string[] =>
      value ? [] : [message],
};
