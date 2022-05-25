import { useEffect, useMemo, useRef, useState } from 'react';

export enum ScreenBreakpoints {
  Phone = 640,
  Tablet = 1024,
  Laptop = 1280,
  Desktop = Number.MAX_SAFE_INTEGER,
}

interface ScreenBreakpointItem<TKey extends string, TValue extends number> {
  name: TKey;
  size: TValue;
}

export const useScreenBreakpointsCustom = <
  TKey extends string,
  TValue extends number
>(
  element: HTMLElement | undefined | null,
  sizes: Record<TKey, TValue>
): TValue | undefined => {
  const sizeMap = useMemo<ScreenBreakpointItem<TKey, TValue>[]>(() => {
    const map: ScreenBreakpointItem<TKey, TValue>[] = [];

    for (const [name, size] of Object.entries<TValue>(sizes)) {
      if (!map.find((x) => x.size === size)) {
        map.push({ name: name as TKey, size });
      }
    }

    return map.sort((x, y) => x.size - y.size);
  }, [sizes]);

  const [resizeObserver, setResizeObserver] = useState<
    ResizeObserver | undefined
  >();

  const [currentItem, setCurrentItem] = useState<
    ScreenBreakpointItem<TKey, TValue> | undefined
  >();

  const currentItemKeyRef = useRef<TKey | undefined>();

  useEffect(() => {
    const observer = new ResizeObserver((resizeObserverEntries) => {
      if (!resizeObserverEntries.length) {
        return;
      }

      const { width } = resizeObserverEntries[0].contentRect;

      for (const sizeMapItem of sizeMap) {
        if (width <= sizeMapItem.size) {
          if (currentItemKeyRef.current !== sizeMapItem.name) {
            currentItemKeyRef.current = sizeMapItem.name;
            setCurrentItem(sizeMapItem);
          }
          return;
        }
      }
    });

    setResizeObserver(observer);

    return () => {
      observer.disconnect();
      setResizeObserver(undefined);
    };
  }, [sizeMap]);

  useEffect(() => {
    if (element && resizeObserver) {
      resizeObserver.observe(element);
    }

    return () => {
      if (element && resizeObserver) {
        resizeObserver.unobserve(element);
      }
    };
  }, [element, resizeObserver]);

  return currentItem?.size;
};

export const useScreenBreakpoints = (element?: HTMLElement) =>
  useScreenBreakpointsCustom(element || document.body, ScreenBreakpoints);
