import {
  createContext,
  memo,
  ReactNode,
  ChangeEvent,
  useContext,
  useState,
  Dispatch,
  SetStateAction,
  FormEvent,
  useCallback,
  CSSProperties,
} from 'react';

export interface CustomFormState {
  fields: Field[];
  fieldsById: Record<string, Field>;
  values: Record<string, string | boolean>;
  valid: boolean;
  touched: boolean;
  validations: Record<string, ((x: unknown) => string[])[]>;
}

interface Field {
  id: string;
  name: string;
  value: unknown;
  touched: boolean;
  errorMessages: string[];
}

const defaultField: Field = {
  value: undefined,
  touched: false,
  errorMessages: [],
  id: undefined as unknown as string,
  name: undefined as unknown as string,
};

const defaultFormState: CustomFormState = {
  fields: [],
  fieldsById: {},
  values: {},
  valid: true,
  touched: false,
  validations: {},
};

const CustomFormContext = createContext<
  [CustomFormState, Dispatch<SetStateAction<CustomFormState>>]
>(
  undefined as unknown as [
    CustomFormState,
    Dispatch<SetStateAction<CustomFormState>>
  ]
);

interface CustomFormProps {
  children: ReactNode;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  onSubmit: (formValues: any) => void | Promise<void>;

  validations?: Record<string, ((x: unknown) => string[])[]>;

  className?: string | undefined;

  style?: CSSProperties | undefined;
}

export const CustomForm = memo(
  ({ children, onSubmit, validations, className, style }: CustomFormProps) => {
    const contextValue = useState(() => ({
      ...defaultFormState,
      validations: validations || {},
    }));

    const handleOnSubmit = useCallback(
      (ev: FormEvent<HTMLFormElement>) => {
        ev.preventDefault();

        const [formState, setFormState] = contextValue;

        if (
          formState.fields.filter((x) => x.touched).length !==
          formState.fields.length
        ) {
          setFormState((prevState) => {
            const newFields = prevState.fields.map((y) => {
              if (y.touched) {
                return y;
              }

              return {
                ...y,
                touched: true,
              };
            });

            return {
              ...prevState,
              fields: newFields,
              fieldsById: Object.fromEntries(newFields.map((y) => [y.id, y])),
              touched: true,
            };
          });
        }

        if (!formState.valid) {
          return;
        }

        void onSubmit(formState.values);
      },
      [onSubmit, contextValue]
    );

    return (
      <CustomFormContext.Provider value={contextValue}>
        <form style={style} className={className} onSubmit={handleOnSubmit}>
          {children}
        </form>
      </CustomFormContext.Provider>
    );
  }
);

const fieldsEqual = (x: Field, y: Field): boolean =>
  x.touched === y.touched &&
  x.name === y.name &&
  x.id === y.id &&
  x.value === y.value &&
  x.errorMessages === y.errorMessages;

const updateField = (
  formState: CustomFormState,
  newField: Field
): CustomFormState => {
  const oldField = formState.fieldsById[newField.id];
  if (!oldField) {
    const newFields = [...formState.fields, newField];
    const newFieldsById = { ...formState.fieldsById, [newField.id]: newField };
    return {
      fields: newFields,
      fieldsById: newFieldsById,
      touched: newFields.some((x) => x.touched),
      valid: !newFields.filter((x) => !!x.errorMessages.length).length,
      values: Object.fromEntries(
        newFields.map((x) => [x.id, x.value])
      ) as Record<string, string | boolean>,
      validations: formState.validations,
    };
  }
  if (fieldsEqual(oldField, newField)) {
    return formState;
  } else {
    const newFields = [
      ...formState.fields.filter((x) => x.id !== newField.id),
      newField,
    ];
    const newFieldsById = { ...formState.fieldsById, [newField.id]: newField };
    return {
      fields: newFields,
      fieldsById: newFieldsById,
      touched: newFields.some((x) => x.touched),
      valid: !newFields.filter((x) => !!x.errorMessages.length).length,
      values: Object.fromEntries(
        newFields.map((x) => [x.id, x.value])
      ) as Record<string, string | boolean>,
      validations: formState.validations,
    };
  }
};

interface FieldInputProps {
  id: string;
  name: string;
  value: unknown;
  onChange: (ev: ChangeEvent<HTMLInputElement>) => void;
  onBlur: () => void;
  error: boolean;
  helperText: string | undefined;
}

interface CustomFieldProps {
  id: string;
  render: (props: FieldInputProps) => unknown;
  validations?: ((x: unknown) => string[])[];
}

export const CustomField = memo(
  ({ id, render, validations }: CustomFieldProps) => {
    const [formState, setFormState] = useContext(CustomFormContext);
    let field = formState.fieldsById[id];
    if (!field) {
      field = {
        ...defaultField,
        id,
        name: id,
        errorMessages: (validations || [])
          .concat(formState.validations[id] || [])
          .flatMap((x) => x(defaultField.value)),
      };
      setTimeout(() => {
        setFormState((oldState) => updateField(oldState, field));
      }, 0);
    }

    const handleOnChange = useCallback(
      (ev: ChangeEvent<HTMLInputElement>) => {
        const newValue =
          ev.target.type === 'checkbox' ? ev.target.checked : ev.target.value;

        setFormState((oldState) =>
          updateField(oldState, {
            ...field,
            value: newValue,
            errorMessages: (validations || [])
              .concat(formState.validations[field.id] || [])
              .flatMap((x) => x(newValue)),
            touched: true,
          })
        );
      },
      [field, formState.validations, setFormState, validations]
    );

    const handleOnBlur = useCallback(() => {
      if (field.touched) {
        return;
      }
      setFormState((oldState) =>
        updateField(oldState, { ...field, touched: true })
      );
    }, [field, setFormState]);

    return (
      <>
        {render({
          id,
          name: id,
          value: field.value || '',
          onBlur: handleOnBlur,
          onChange: handleOnChange,
          error: !!field.errorMessages.length && field.touched,
          helperText:
            !!field.errorMessages.length && field.touched
              ? field.errorMessages.join(',')
              : undefined,
        })}
      </>
    );
  }
);
