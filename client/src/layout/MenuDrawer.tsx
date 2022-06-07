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

import { useAppDispatch, useAppSelector } from '~infrastructure/redux-store';
import { menuDrawerActions, menuDrawerSelector } from '~layout/menuDrawerSlice';

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

  const { open } = useAppSelector(menuDrawerSelector);

  const handleOnIconClick = useCallback(() => {
    dispatch(menuDrawerActions.toggleDrawer());
  }, [dispatch]);

  return (
    <Drawer
      className={`flex-shrink-0 ${customDrawerClassName}`}
      variant="persistent"
      anchor="left"
      open={open}
    >
      <div className={`flex items-center justify-between py-1 drawer-header`}>
        <div className="flex-auto text-center font-bold select-none">Menu</div>
        <IconButton onClick={handleOnIconClick}>
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
