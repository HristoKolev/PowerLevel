// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { validations, mergeValidations } from './validations';

export const rpcValidations = {
  loginRequest: {
    emailAddress: mergeValidations([ validations.required('The email address field is required.') ]),
    password: mergeValidations([ validations.required('The password field is required.') ]),
    rememberMe: {},
    recaptchaToken: {},
  },
  pingRequest: {
  },
  profileInfoRequest: {
    count: {},
  },
  registerRequest: {
    emailAddress: mergeValidations([ validations.email('Please, enter a valid email address.'), validations.required('The email address field is required.') ]),
    password: mergeValidations([ validations.required('The password field is required.'), validations.stringLength(10, 40, 'The password must be between 10 and 40 characters long.') ]),
    recaptchaToken: {},
  },
};