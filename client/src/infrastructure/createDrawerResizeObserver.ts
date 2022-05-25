import { ResponsiveDrawerBreakpoint } from '~components/menuDrawerSlice';

export const createDrawerResizeObserver = (
  element: Element,
  callback: (breakpoint: ResponsiveDrawerBreakpoint) => void
): void => {
  const allSizes = (
    Object.values(ResponsiveDrawerBreakpoint).filter(
      (x) => typeof x === 'number'
    ) as number[]
  ).sort((x, y) => x - y);

  let currentSize: number | undefined = undefined;

  const observer = new ResizeObserver((resizeObserverEntries) => {
    if (!resizeObserverEntries.length) {
      return;
    }
    const { width } = resizeObserverEntries[0].contentRect;
    for (const size of allSizes) {
      if (width <= size) {
        if (currentSize !== size) {
          currentSize = size;
          callback(size);
        }
        return;
      }
    }
  });

  observer.observe(element);
};
