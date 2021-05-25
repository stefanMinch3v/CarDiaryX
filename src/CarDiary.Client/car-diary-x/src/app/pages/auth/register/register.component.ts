import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { SettingsService } from '../../../core/services/settings.service';
import { loadingOverlays } from '../../../core/constants/loadingOverlays';
import { AuthService } from '../../../core/services/auth.service';
import { IdentityService } from '../../../core/services/identity.service';
import { FormValidator } from '../../../core/helpers/form-validator';

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
    private settingsService: SettingsService,
    private translateService: TranslateService) { }

  ngOnInit(): void {
    this.langSub = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));

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
      validators: [FormValidator.matchPasswords, FormValidator.matchEmail]
    });
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }

  ionViewWillLeave(): void {
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
      }, () => loading.dismiss(), () => loading.dismiss());
  }
}
