const emailRegex =
  /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+\.)+[^<>()[\].,;:\s@"]{2,})$/i;

export const validations = {
  required:
    (message: string) =>
    (value: unknown): string[] =>
      value || value === 0 ? [] : [message],
  email:
    (message: string) =>
    (value: unknown): string[] => {
      const stringValue = String(value || '');
      return !value || emailRegex.test(stringValue) ? [] : [message];
    },
  stringLength:
    (min: number, max: number, message: string) =>
    (value: unknown): string[] => {
      const length = String(value || '').length;
      return length < min || length > max ? [message] : [];
    },
};
