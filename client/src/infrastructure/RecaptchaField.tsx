import { memo, useCallback, CSSProperties } from 'react';
import { useController, Control } from 'react-hook-form';
import ReCAPTCHA from 'react-google-recaptcha';

interface RecaptchaFieldProps {
  name: string;
  control: unknown;
  className?: string;
  style?: CSSProperties;
}

export const RecaptchaField = memo(
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  ({ name, control, style, className }: RecaptchaFieldProps): JSX.Element => {
    const controller = useController({ name, control: control as Control });

    const handleOnChanged = useCallback(
      (token: string | null) => {
        controller.field.onChange(token);
      },
      [controller.field]
    );

    return (
      <ReCAPTCHA
        className={className}
        style={style}
        sitekey="6Ld9TPogAAAAAP7Lm69GVCJiQTsyPAZDCg05LsyF"
        onChange={handleOnChanged}
      />
    );
  }
);
