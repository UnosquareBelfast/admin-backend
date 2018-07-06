import React from 'react';
import container from './container';
import { PropTypes as PT } from 'prop-types';
import {
  FormGroup,
  Label,
  TextBox,
  TextBoxLarge,
  DropdownContainer,
  Dropdown,
  DatePickerContainer,
} from './styled';
import DatePicker from 'react-datepicker';

const Input = props => {
  let inputElement = null;
  let inputClasses = [];
  const {
    label,
    valid,
    touched,
    type,
    htmlAttr,
    value,
    changed,
    focus,
  } = props;
  let formGroupId = label;
  let id = formGroupId.replace(' ', '').toLowerCase();

  if (!valid && touched) {
    inputClasses.push('invalid');
  }

  switch (type) {
    case 'input':
      inputElement = (
        <TextBox
          className={inputClasses.join(' ')}
          id={id}
          {...htmlAttr}
          value={value}
          onChange={changed}
          autoFocus={focus}
        />
      );
      break;
    case 'textarea':
      inputElement = (
        <TextBoxLarge
          rows={6}
          className={'large' + inputClasses.join(' ')}
          {...htmlAttr}
          value={value}
          id={id}
          onChange={changed}
          autoFocus={focus}
        />
      );
      break;
    case 'checkbox':
      inputElement = (
        <TextBox
          className={inputClasses.join(' ')}
          {...htmlAttr}
          checked={value}
          id={id}
          onChange={changed}
          autoFocus={focus}
        />
      );
      break;
    case 'select':
      inputElement = (
        <DropdownContainer className={inputClasses.join(' ')}>
          <Dropdown
            className={inputClasses.join(' ')}
            value={value}
            id={id}
            onChange={changed}
          >
            {htmlAttr.options.map(option => (
              <option key={option.value} value={option.value}>
                {option.displayValue}
              </option>
            ))}
          </Dropdown>
        </DropdownContainer>
      );
      break;
    case 'date':
      inputElement = (
        <DatePickerContainer className={inputClasses.join(' ')}>
          <DatePicker selected={value} onChange={changed} />
        </DatePickerContainer>
      );
      break;
    default:
      inputElement = (
        <TextBox
          className={inputClasses.join(' ')}
          {...htmlAttr}
          value={value}
          id={id}
          onChange={changed}
          autoFocus={focus}
        />
      );
  }

  return (
    <FormGroup className={type == 'checkbox' ? 'checkbox' : null}>
      <Label htmlFor={id}>{label}</Label>
      {inputElement}
    </FormGroup>
  );
};

Input.propTypes = {
  htmlAttr: PT.object.isRequired,
  type: PT.string.isRequired,
  value: PT.any,
  label: PT.string.isRequired,
  changed: PT.func.isRequired,
  valid: PT.bool.isRequired,
  touched: PT.bool,
  focus: PT.bool,
};

Input.defaultProps = {
  touched: false,
  focus: false,
};

export default container(Input);
