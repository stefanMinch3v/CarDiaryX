import { AbstractControl, ValidationErrors } from '@angular/forms';

export class FormValidator {
  static matchPasswords(abstractControl: AbstractControl): ValidationErrors | null {
    if (!abstractControl) {
      return null;
    }

    const password = abstractControl.get('password');
    const confirmPassword = abstractControl.get('confirmPassword');

    if (confirmPassword.errors && !confirmPassword.errors.mustMatch) {
      // return if another validator has already found an error on the confirmPassword
      return;
    }

    // set error on confirmPassword if validation fails
    if (password.value !== confirmPassword.value) {
        confirmPassword.setErrors({ mustMatch: true });
    } else {
        confirmPassword.setErrors(null);
    }
  }

  static matchEmail(abstractControl: AbstractControl): ValidationErrors | null {
    if (!abstractControl) {
      return null;
    }

    const email = abstractControl.get('email');

    var re = /\S+@\S+\.\S+/;
    const valid = re.test(email.value);

    if (!valid) {
      email.setErrors({ mustMatch: true });
    } else {
      email.setErrors(null);
    }
  }
}