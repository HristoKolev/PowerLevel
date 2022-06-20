import { memo, useCallback } from 'react';
import { Link } from 'react-router-dom';
import { css } from '@linaria/core';
import { IconButton, Toolbar, AppBar, Button } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';

import { useAppDispatch, useAppSelector } from '~infrastructure/redux';

import {
  layoutActions,
  layoutSelector,
  ResponsiveDrawerBreakpoint,
} from './layoutSlice';

const customAppBarClassName = css`
  transition: margin 190ms cubic-bezier(0.4, 0, 0.6, 1) 0ms,
    width 190ms cubic-bezier(0.4, 0, 0.6, 1) 0ms;

  &.drawer-side-by-side.drawer-open {
    width: calc(100% - var(--custom-drawer-width));
    margin-left: var(--custom-drawer-width);
  }

  &.drawer-open {
    transition: margin 200ms cubic-bezier(0, 0, 0.2, 1) 0ms,
      width 200ms cubic-bezier(0, 0, 0.2, 1) 0ms;
  }

  .brand-name {
    font-size: 1.25rem;
    line-height: 1.6;
  }
`;

export const CustomAppBar = memo((): JSX.Element => {
  const dispatch = useAppDispatch();

  const { open, breakpoint } = useAppSelector(layoutSelector);

  const handleOnIconClick = useCallback(() => {
    dispatch(layoutActions.toggleDrawer());
  }, [dispatch]);

  return (
    <AppBar
      position="fixed"
      className={`${customAppBarClassName} ${open ? 'drawer-open' : ''} ${
        breakpoint === ResponsiveDrawerBreakpoint.SideBySide
          ? 'drawer-side-by-side'
          : ''
      }`}
    >
      <Toolbar>
        <IconButton
          color="inherit"
          aria-label="open drawer"
          onClick={handleOnIconClick}
          edge="start"
          className={`mr-2 ${open ? 'hidden' : ''}`}
        >
          <MenuIcon data-testid="app-bar-drawer-toggle" />
        </IconButton>

        <div className="font-medium ml-3 brand-name flex-grow">
          <Link to="/" className="link" data-testid="brand-name">
            Power Level
          </Link>
        </div>

        <Link to="/login" className="link">
          <Button color="inherit">Login</Button>
        </Link>
      </Toolbar>
    </AppBar>
  );
});
