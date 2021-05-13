import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { I18nService } from '../../../core/services/i18n.service';
import { loadingOverlays } from '../../../core/constants/loadingOverlays';
import { AuthService } from '../../../core/services/auth.service';
import { IdentityService } from '../../../core/services/identity.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit, OnDestroy {
  private langSub: Subscription;
  registerForm: FormGroup;

  constructor(
    private modalCntrl: ModalController, 
    private identityService: IdentityService,
    private loadingCntrl: LoadingController,
    private authService: AuthService,
    private router: Router,
    private i18nService: I18nService,
    private translateService: TranslateService) { }

  ngOnInit(): void {
    this.langSub = this.i18nService.currentLanguage.subscribe(lang => this.translateService.use(lang));

    this.registerForm = new FormGroup({
      firstName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      lastName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      age: new FormControl(null, {
        validators: [Validators.required, Validators.min(16), Validators.max(120)]
      }),
      email: new FormControl(null, {
        validators: [Validators.required, Validators.email]
      }),
      password: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(6)]
      }),
      confirmPassword: new FormControl(null)
    }, {
      validators: [this.matchPasswords, this.matchEmail]
    });
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }

  get f() { return this.registerForm.controls; }
  
  onDismissModal(): void {
    this.modalCntrl.dismiss({ 'dismissed': true });
  }

  async onSubmit(): Promise<void> {
    if (!this.registerForm.valid) {
      return;
    }

    const firstName = this.registerForm.value.firstName;
    const lastName = this.registerForm.value.lastName;
    const age = this.registerForm.value.age;
    const email = this.registerForm.value.email;
    const password = this.registerForm.value.password;

    const loading = await this.loadingCntrl.create({ keyboardClose: true, id: loadingOverlays.initialRegistration });
    await loading.present();

    this.identityService.register({ firstName, lastName, age, email, password })
      .pipe(
        switchMap(_ => this.identityService.login({ email, password })))
      .subscribe(response => {
        const token = response.token;
        const expiration = response.expiration;

        this.authService.authenticateUser(token, expiration);
        this.router.navigate(['tabs']);

        this.onDismissModal();
      }, () => loading.dismiss());
  }

  private matchPasswords(abstractControl: AbstractControl): ValidationErrors | null {
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

  private matchEmail(abstractControl: AbstractControl): ValidationErrors | null {
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
