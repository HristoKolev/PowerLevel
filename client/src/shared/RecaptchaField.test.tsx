import { memo, useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Button } from '@mui/material';
import ReCAPTCHA, { ReCAPTCHAProps } from 'react-google-recaptcha';
import userEvent from '@testing-library/user-event';
import { screen, act } from '@testing-library/react';

import { renderWithProviders } from '~test-utils';

import { RecaptchaField } from './RecaptchaField';

jest.mock('react-google-recaptcha', () => jest.fn(() => null));

const ReCAPTCHAMock = ReCAPTCHA as jest.Mock;

interface TestValues {
  field1: string;
}

const TestForm = memo(
  ({ onSubmit }: { onSubmit: (values: TestValues) => void }) => {
    const { handleSubmit, control } = useForm<TestValues>({
      mode: 'onTouched',
    });

    const handleOnSubmit = useMemo(
      () => handleSubmit(onSubmit),
      [onSubmit, handleSubmit]
    );

    return (
      // eslint-disable-next-line @typescript-eslint/no-misused-promises
      <form onSubmit={handleOnSubmit}>
        <RecaptchaField control={control} name="field1" />
        <Button type="submit" data-testid="submit-button">
          Submit
        </Button>
      </form>
    );
  }
);

afterEach(() => {
  jest.resetAllMocks();
  jest.restoreAllMocks();
  jest.resetModules();
});

test('sets the correct field value', async () => {
  const user = userEvent.setup();

  let props: ReCAPTCHAProps = { onChange: () => {}, sitekey: '' };
  ReCAPTCHAMock.mockImplementation((x) => {
    props = x as ReCAPTCHAProps;
    return null;
  });

  const handleOnSubmit = jest.fn();

  renderWithProviders(<TestForm onSubmit={handleOnSubmit} />);

  expect(props.sitekey).toEqual('6Ld9TPogAAAAAP7Lm69GVCJiQTsyPAZDCg05LsyF');

  const token = '__TOKEN__';

  act(() => {
    const onChange = props.onChange as (token: string) => void;
    onChange(token);
  });

  await user.click(await screen.findByTestId('submit-button'));

  expect(handleOnSubmit).toHaveBeenCalledWith(
    { field1: token },
    expect.any(Object)
  );
});
