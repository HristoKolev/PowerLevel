const emailRegex =
  /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+\.)+[^<>()[\].,;:\s@"]{2,})$/i;

export const validations = {
  required: (message: string) => ({
    required: {
      value: true,
      message,
    },
  }),
  email: (message: string) => ({
    pattern: {
      value: emailRegex,
      message,
    },
  }),
  stringLength: (min: number, max: number, message: string) => [
    {
      minLength: {
        value: min,
        message,
      },
      maxLength: {
        value: max,
        message,
      },
    },
  ],
};

export const mergeValidations = (all: unknown[]): Record<string, unknown> =>
  all
    .flat()
    .reduce((x, y) =>
      Object.assign(x as Record<string, unknown>, y as Record<string, unknown>)
    ) as Record<string, unknown>;
