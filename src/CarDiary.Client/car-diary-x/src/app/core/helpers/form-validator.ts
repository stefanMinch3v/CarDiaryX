import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

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

  static matchValidAddressInDenmark(addressType: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control || !addressType) {
        return null;
      }


      const address = control.get(addressType);

      if (address.errors && address.errors.required) {
        // return if another validator has already found an error in this case required
        return;
      }

      if (address.value && address.value.notFullAddress) {
        address.setErrors({ missingId: true });
      } else {
        address.setErrors(null);
      }
    };
  }

  static matchDepartureArrivalDates(abstractControl: AbstractControl): ValidationErrors | null {
    if (!abstractControl) {
      return null;
    }

    const departureDate = abstractControl.get('departureDate');
    const arrivalDate = abstractControl.get('arrivalDate');

    if (departureDate.errors && departureDate.errors.required || arrivalDate.errors && arrivalDate.errors.required) {
      // return if another validator has already found an error
      return;
    }

    const parseDeparture = Date.parse(departureDate.value);
    const parseArrival = Date.parse(arrivalDate.value);

    if (parseDeparture > parseArrival) {
      departureDate.setErrors({ invalidRange: true });
      arrivalDate.setErrors({ invalidRange: true });
    } else {
      departureDate.setErrors(null);
      arrivalDate.setErrors(null);
    }
  }
}