// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { validations } from './validations';

export const rpcValidations = {
  loginRequest: {
    emailAddress: [ validations.required('The email address field is required.') ],
    password: [ validations.required('The password field is required.') ],
    rememberMe: [],
  },
  pingRequest: {
  },
  profileInfoRequest: {
    count: [],
  },
  registerRequest: {
    emailAddress: [ validations.email('Please, enter a valid email address.'), validations.required('The email address field is required.') ],
    password: [ validations.required('The password field is required.'), validations.stringLength(10, 40, 'The password must be between 10 and 40 characters long.') ],
  },
};
