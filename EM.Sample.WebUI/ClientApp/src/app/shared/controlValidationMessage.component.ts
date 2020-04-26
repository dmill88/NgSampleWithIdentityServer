import { Component, Input } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ControlValidationService } from './../shared/controlValidation.service';

@Component({
  selector: 'control-validation-message',
  template: `<div *ngIf="errorMessage !== null" class="has-error has-feedback"><label class="control-label">{{errorMessage}}</label></div>`,
  providers: []
})
export class ControlValidationMessageComponent {
  @Input() control: FormControl;
  @Input() show: boolean;

  constructor() { }

  get errorMessage(): string {
    if (this.show) {
      for (let propertyName in this.control.errors) {
        if (this.control.errors.hasOwnProperty(propertyName)) {
          return ControlValidationService.getValidatorErrorMessage(propertyName, this.control.errors[propertyName]);
        }
      }
    }
    return null;
  }
}
