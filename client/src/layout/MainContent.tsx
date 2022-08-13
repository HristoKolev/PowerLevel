import { memo, ReactNode } from 'react';
import { css } from '@linaria/core';

import { useAppSelector } from '~infra/redux';

import { layoutSelector, ResponsiveDrawerBreakpoint } from './layoutSlice';

const mainContentClassName = css`
  transition: margin 190ms cubic-bezier(0.4, 0, 0.6, 1) 0ms;
  margin-left: 0;

  &.drawer-open {
    transition: margin 200ms cubic-bezier(0, 0, 0.2, 1) 0ms;
  }

  &.drawer-side-by-side.drawer-open {
    margin-left: var(--custom-drawer-width);
  }

  margin-top: 56px;

  @media (min-width: 600px) {
    margin-top: 64px;
  }
`;

export interface MainContentProps {
  children: ReactNode;
}

export const MainContent = memo(
  ({ children }: MainContentProps): JSX.Element => {
    const { open, breakpoint } = useAppSelector(layoutSelector);
    return (
      <main
        className={`flex-auto flex flex-col p-3 ${mainContentClassName} ${
          open ? 'drawer-open' : ''
        } ${
          breakpoint === ResponsiveDrawerBreakpoint.SideBySide
            ? 'drawer-side-by-side'
            : ''
        }`}
      >
        {children}
      </main>
    );
  }
);
