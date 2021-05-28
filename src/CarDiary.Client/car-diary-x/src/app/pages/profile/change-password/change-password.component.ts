import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoadingController, ModalController, Platform } from '@ionic/angular';
import { IdentityService } from '../../../core/services/identity.service';
import { FormValidator } from '../../../core/helpers/form-validator';
import { ToastService } from '../../../core/services/toast.service';
import { validations } from '../../../core/constants/validations';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent implements OnInit {
  isIOS: boolean;
  changePasswordForm: FormGroup;

  constructor(
    private platform: Platform,
    private modalCntrl: ModalController,
    private loadingCntrl: LoadingController,
    private toastService: ToastService,
    private identityService: IdentityService) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');

    this.changePasswordForm = new FormGroup({
      currentPassword: new FormControl(null, {
        validators: [Validators.required]
      }),
      password: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(validations.user.PASSWORD_MIN_LENGTH), Validators.maxLength(validations.user.PASSWORD_MAX_LENGTH)]
      }),
      confirmPassword: new FormControl(null)
    }, {
      validators: [FormValidator.matchPasswords]
    });
  }

  get f() { return this.changePasswordForm.controls; }

  onDismissModal(): void {
    this.modalCntrl.dismiss({ 'dismissed': true });
  }

  async onSubmit(): Promise<void> {
    if (!this.changePasswordForm.valid) {
      return;
    }

    const currentPassword = this.changePasswordForm.value.currentPassword;
    const newPassword = this.changePasswordForm.value.password;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.identityService.changePassword({ currentPassword, newPassword })
      .subscribe(_ => {
        this.onDismissModal();
        this.toastService.presentSuccessToast();
      }, () => loading.dismiss(), () => loading.dismiss());
  }
}
