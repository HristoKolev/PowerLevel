import { ReactNode, memo, useEffect } from 'react';

import { useAppDispatch } from '~infrastructure/redux';

import { CustomAppBar } from './CustomAppBar';
import { MenuDrawer } from './MenuDrawer';
import { MainContent } from './MainContent';
import { createDrawerResizeObserver } from './createDrawerResizeObserver';
import { layoutActions } from './layoutSlice';

interface LayoutProps {
  children: ReactNode;
}

export const Layout = memo(({ children }: LayoutProps): JSX.Element => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    const root = document.getElementById('root') as Element;

    const observer = createDrawerResizeObserver(root, (breakpoint) => {
      dispatch(layoutActions.changeDrawerBreakpoint({ breakpoint }));
    });

    return () => {
      observer.disconnect();
    };
  }, [dispatch]);

  return (
    <>
      <CustomAppBar />
      <MenuDrawer />
      <MainContent>{children}</MainContent>
    </>
  );
});
