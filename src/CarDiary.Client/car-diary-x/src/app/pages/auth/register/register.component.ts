import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { SettingsService } from '../../../core/services/settings.service';
import { AuthService } from '../../../core/services/auth.service';
import { IdentityService } from '../../../core/services/identity.service';
import { FormValidator } from '../../../core/helpers/form-validator';
import { validations } from '../../../core/constants/validations';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit, OnDestroy {
  private langSub$: Subscription;
  registerForm: FormGroup;

  constructor(
    private modalCntrl: ModalController, 
    private identityService: IdentityService,
    private loadingCntrl: LoadingController,
    private authService: AuthService,
    private router: Router,
    private settingsService: SettingsService,
    private translateService: TranslateService) { }

  ngOnInit(): void {
    this.langSub$ = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));

    this.registerForm = new FormGroup({
      firstName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(validations.user.NAME_MIN_LENGTH), Validators.maxLength(validations.user.NAME_MAX_LENGTH)]
      }),
      lastName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(validations.user.NAME_MIN_LENGTH), Validators.maxLength(validations.user.NAME_MAX_LENGTH)]
      }),
      email: new FormControl(null, {
        validators: [Validators.required, Validators.email, Validators.minLength(validations.user.EMAIL_MIN_LENGTH), Validators.maxLength(validations.user.EMAIL_MAX_LENGTH)]
      }),
      password: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(validations.user.PASSWORD_MIN_LENGTH), Validators.maxLength(validations.user.PASSWORD_MAX_LENGTH)]
      }),
      confirmPassword: new FormControl(null)
    }, {
      validators: [FormValidator.matchPasswords, FormValidator.matchEmail]
    });
  }

  ngOnDestroy(): void {
    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }
  }

  ionViewWillLeave(): void {
    if (this.langSub$) {
      this.langSub$.unsubscribe();
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
    const email = this.registerForm.value.email;
    const password = this.registerForm.value.password;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.identityService.register({ firstName, lastName, email, password })
      .pipe(
        switchMap(_ => this.identityService.login({ email, password })))
      .subscribe(response => {
        const token = response?.token;
        const expiration = response?.expiration;

        this.authService.authenticateUser(token, expiration);
        this.router.navigate(['tabs']);

        this.onDismissModal();
      }, () => loading.dismiss(), () => loading.dismiss());
  }
}
