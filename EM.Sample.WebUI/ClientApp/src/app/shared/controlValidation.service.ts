import { AbstractControl, Validators, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import * as _ from "lodash";
import { ListItemNameId, SelectListItem } from './../models/formControl.models';

export class ControlValidationService {
  static getValidatorErrorMessage(validatorName: string, validatorValue?: any) {
    let validationMessage: string = null;
    const config = {
      'required': 'Required',
      'email': 'Invalid email address',
      'minlength': `Minimum length ${validatorValue.requiredLength}`,
      'maxlength': `Maximum length ${validatorValue.requiredLength}`,
      'min': `Minimum value is ${validatorValue.min}`,
      'max': `Maximum value is ${validatorValue.max}`,
      'noItemInTheList': `You must select a new value`,
      'pattern': `Invalid value`,
      'invalidCreditCard': 'Is invalid credit card number',
      'invalidEmailAddress': 'Invalid email address',
      'invalidPassword': 'Invalid password. Password must be at least 6 characters long, and contain a number.',
      'invalidStartDate': 'Invalid start date.',
    };
    validationMessage = config[validatorName];
    if (!validationMessage) {
      validationMessage = `Unknow validator key: ${validatorName}`;
    }

    return validationMessage;
  }

  static parseWebApiErrors(errorObj: any): Array<string> {
    let errors: Array<string> = new Array<string>();
    let errorMsg: string;
    let innerException: any = null;

    //debugger;

    if (errorObj) {
      let modelStateErrors: any = errorObj.ModelState;
      if (modelStateErrors == null && errorObj.error && errorObj.error.ModelState) {
        modelStateErrors = errorObj.error.ModelState;
      }

      if (modelStateErrors) {
        for (let propName in modelStateErrors) {
          for (errorMsg of modelStateErrors[propName]) {
            let index = propName.lastIndexOf(".");
            if (index > 0 && propName.length > index + 1) {
              propName = propName.substr(index + 1);
            }
            let propNameReplacement = _.startCase(propName).replace(' ID', '').replace(' Id', '');
            errorMsg = errorMsg.replace(propName, propNameReplacement);
            errors.push(errorMsg);
          }
        }
      } else if (errorObj.error && errorObj.error.ExceptionMessage) {
        debugger;
        errorMsg = errorObj.error.ExceptionMessage;
        errors.push(errorMsg);
      } else if (errorObj.error) {
        errorMsg = errorObj.error;
        errors.push(errorMsg);
      } else if (errorObj.message) {
        errors.push(errorObj.message);
      } else if (errorObj.ExceptionMessage) {
        errorMsg = errorObj.ExceptionMessage;
        innerException = errorObj.InnerException;
        while (innerException && innerException.ExceptionMessage) {
          errorMsg = innerException.ExceptionMessage;
          innerException = innerException.InnerException;
        }
        errors.push(errorMsg);
      } else if (errorObj._body) {
        let expBody = JSON.parse(errorObj._body);
        if (expBody.ExceptionMessage) {
          errors.push(expBody.ExceptionMessage);
        }
        else if (expBody.Message) {
          errors.push(expBody.Message);
        }
      } else if (errorObj.Errors) {
        for (let propName in errorObj.Errors) {
          if (errorObj.Errors[propName].errors) {
            errors.push(errorObj.Errors[propName].errors[0]);
          }
        };
      } else if (errorObj.Message) {
        errors.push(errorObj.Message);
      } else { // This method is sometimes used to parse for errors on success results, so do not add default error message. 
        //debugger;
      }
    }

    return errors;
  }

  static requiredIdValidator(control: AbstractControl): ValidationErrors {
    if (control.value) {
      const id: number = control.value;
      if (id === 0) {
        return { 'required': true };
      }
      return null;
    } else {
      return { 'required': true };
    }
  }

  static startDateValidator(control: AbstractControl): ValidationErrors {
    if (control.value) {
      const startDate: Date = control.value;
      if (startDate.getDay() !== 1) {
        return { 'invalidStartDate': true };
      }
      return null;
    } else {
      return null;
    }
  }

  static creditCardValidator(control: AbstractControl): ValidationErrors {
    // Visa, MasterCard, American Express, Diners Club, Discover, JCB
    if (control.value && control.value.match(/^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/)) {
      return null;
    } else {
      return { 'invalidCreditCard': true };
    }
  }

  static emailValidator(control: AbstractControl): ValidationErrors {
    // RFC 2822 compliant regex
    if (control.value && control.value.match(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)) {
      return null;
    } else {
      return { 'invalidEmailAddress': true };
    }
  }

  static passwordValidator(control: AbstractControl): ValidationErrors {
    // {6,100}           - Assert password is between 6 and 100 characters
    // (?=.*[0-9])       - Assert a string has at least one number
    if (control.value && control.value.match(/^(?=.*[0-9])[a-zA-Z0-9!@#$%^&*]{6,100}$/)) {
      return null;
    } else {
      return { 'invalidPassword': true };
    }
  }

  static noItemInNameIdListValidator(id: number, list: Array<ListItemNameId>): ValidatorFn {
    console.log("noItemInNameIdListValidator called");
    return (control: AbstractControl): { [key: string]: any } => {
      console.log("noItemInNameIdListValidator validator called with control.value = " + control.value);
      let notFound: boolean = false;
      if (control.value != null && list != null && list.length > 0) {
        console.log("noItemInNameIdListValidator check called");
        let id: number = control.value;
        notFound = list.find(i => i.id == id) == null;
      }
      return notFound ? { 'noItemInTheList': { value: control.value } } : null;
    };
  }

  static noItemInSelectListItemListValidator(id: number, list: Array<SelectListItem>): ValidatorFn {
    console.log("noItemInSelectListItemListValidator called");
    return (control: AbstractControl): { [key: string]: any } => {
      console.log("noItemInSelectListItemListValidator validator called with control.value = " + control.value);
      let notFound: boolean = false;
      if (control.value != null && list != null && list.length > 0) {
        console.log("noItemInSelectListItemListValidator check called");
        let id: number = control.value;
        notFound = list.find(i => i.value == id) == null;
      }
      return notFound ? { 'noItemInTheList': { value: control.value } } : null;
    };
  }

  static getValidationFormErrors(form: FormGroup, displayNames: Map<string, string>): Array<string> {
    let errorMessages: Array<string> = new Array<string>();
    let isValid = true;
    let fieldDisplayName: string;

    for (let key in form.controls) {
      let cntrl = form.get(key);

      if (cntrl.errors) {
        console.log(`${key} ${JSON.stringify(cntrl.errors)}`);
        isValid = false;

        if (displayNames.has(key)) {
          fieldDisplayName = displayNames.get(key);
        }
        else {
          fieldDisplayName = _.startCase(key).replace(' ID', '').replace(' Id', ''); // _.startCase is a lodash method to convert camel-case properties to dispaly format
        }

        //debugger;

        if (cntrl.errors.required) {
          errorMessages.push(`${fieldDisplayName} is required`);
        } else if (cntrl.errors.max) {
          errorMessages.push(`The maximum value of ${fieldDisplayName} is ${cntrl.errors.max.max}.`);
        } else if (cntrl.errors.min) {
          errorMessages.push(`The mimimum value of ${fieldDisplayName} is ${cntrl.errors.min.min}.`);
        } else if (cntrl.errors.maxlength) {
          errorMessages.push(`The maximum length of ${fieldDisplayName} is ${cntrl.errors.maxlength.requiredLength}.`);
        } else if (cntrl.errors.minlength) {
          errorMessages.push(`The minimum length of ${fieldDisplayName} is ${cntrl.errors.minlength.requiredLength}.`);
        } else if (cntrl.errors.invalidStartDate) {
          errorMessages.push(`${fieldDisplayName} must be a Monday.`);
        } else if (cntrl.errors.noItemInTheList) {
          errorMessages.push(`You must select a new value for ${fieldDisplayName}.`);
        } else if (cntrl.errors.invalidCreditCard) {
          errorMessages.push(`Invalid Credit Card value for ${fieldDisplayName}.`);
        }
      }
    }
    return errorMessages;
  }

}
