export const delay = (interval: number): Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, interval));

export const breakpoints = {
  tablet: 640,
  laptop: 1024,
  desktop: 1280,
};
