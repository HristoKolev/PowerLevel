import { memo, useCallback } from 'react';
import {
  Divider,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
} from '@mui/material';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';
import { css } from '@linaria/core';

import { useAppDispatch, useAppSelector } from '~infra/redux';

import { layoutActions, layoutSelector } from './layoutSlice';

const customDrawerClassName = css`
  width: var(--custom-drawer-width);

  .MuiDrawer-paper {
    width: var(--custom-drawer-width);
    box-sizing: border-box;
  }

  .drawer-header {
    min-height: 56px;

    @media (min-width: 600px) {
      min-height: 64px;
    }
  }
`;

export const MenuDrawer = memo((): JSX.Element => {
  const dispatch = useAppDispatch();

  const { open } = useAppSelector(layoutSelector);

  const handleOnIconClick = useCallback(() => {
    dispatch(layoutActions.toggleDrawer());
  }, [dispatch]);

  return (
    <Drawer
      className={`flex-shrink-0 ${customDrawerClassName}`}
      variant="persistent"
      anchor="left"
      open={open}
    >
      <div className={`flex items-center justify-between py-1 drawer-header`}>
        <div
          className="flex-auto text-center font-bold select-none"
          data-testid="drawer-menu-title"
        >
          Menu
        </div>
        <IconButton onClick={handleOnIconClick} data-testid="drawer-toggle">
          <ChevronLeftIcon />
        </IconButton>
      </div>

      <Divider />

      <List>
        {['Inbox', 'Starred', 'Send email', 'Drafts'].map((text, index) => (
          <ListItem key={text}>
            <ListItemIcon>
              {index % 2 === 0 ? <InboxIcon /> : <MailIcon />}
            </ListItemIcon>
            <ListItemText primary={text} />
          </ListItem>
        ))}
      </List>

      <Divider />

      <List>
        {['All mail', 'Trash', 'Spam'].map((text, index) => (
          <ListItem key={text}>
            <ListItemIcon>
              {index % 2 === 0 ? <InboxIcon /> : <MailIcon />}
            </ListItemIcon>
            <ListItemText primary={text} />
          </ListItem>
        ))}
      </List>
    </Drawer>
  );
});
