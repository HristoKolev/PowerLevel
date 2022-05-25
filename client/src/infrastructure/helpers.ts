export const delay = (interval: number): Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, interval));
