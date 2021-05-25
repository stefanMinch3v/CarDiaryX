import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController, LoadingController, ModalController, Platform } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';
import { IdentityService } from '../../core/services/identity.service';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { Subscription } from 'rxjs';
import { UserDetailsModel } from '../../core/models/identity/user-details.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit, OnDestroy {
  private userSub: Subscription;
  updateUserForm: FormGroup;
  userDetails: UserDetailsModel;
  isLoading: boolean;
  isIOS: boolean;

  constructor(
    private location: Location, 
    private platform: Platform,
    private modalCntrl: ModalController,
    private alertCntrl: AlertController,
    private loadingCntrl: LoadingController,
    private translateService: TranslateService,
    private identityService: IdentityService,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');
    this.userSub = this.identityService.get().subscribe(user => { 
      this.isLoading = false;
      this.userDetails = user;
      this.updateUserForm.patchValue(this.userDetails)
    });
    this.updateUserForm = new FormGroup({
      firstName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      lastName: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      age: new FormControl(null, {
        validators: [Validators.required, Validators.min(16), Validators.max(120)]
      }),
    });
  }

  ngOnDestroy(): void {
    if (this.userSub) {
      this.userSub.unsubscribe();
    }
  }
  
  // if account is removed I use router navigate cuz ngOnDestroy does not trigger
  ionViewWillLeave(): void {
    if (this.userSub) {
      this.userSub.unsubscribe();
    }
  }

  get f() { return this.updateUserForm.controls; }

  onNavigateBack(): void {
    return this.location.back();
  }

  async presentChangePasswordModal(): Promise<void> {
    const modal = await this.modalCntrl.create({
      component: ChangePasswordComponent
    });

    return await modal.present();
  }

  async presentDeleteAccountAlert(): Promise<void> {
    const alert = await this.alertCntrl.create({
      cssClass: 'delete-account-alert',
      header: this.translateService.instant('Account will be permanently deleted!'),
      subHeader: this.translateService.instant('All your data link to the account will be erased!'),
      inputs: [
        {
          name: 'password',
          type: 'password',
          placeholder: this.translateService.instant('Confirm Password')
        }
      ],
      buttons: [
        {
          text: this.translateService.instant('Cancel'),
          role: 'cancel'
        }, {
          text: this.translateService.instant('Delete'),
          role: 'delete',
          handler: async (alertData) => {
            const loading = await this.loadingCntrl.create({ keyboardClose: true });
            await loading.present();

            this.identityService.deleteAccount(alertData?.password)
              .subscribe(_ => {
                loading.dismiss();
                this.toastService.presentSuccessToast();
                this.authService.deauthenticateUser();
                this.router.navigate(['/auth']);
              }, () => loading.dismiss());
          }
        }
      ]
    });

    await alert.present();
  }

  async onSubmit(): Promise<void> {
    if (!this.updateUserForm.valid) {
      return;
    }

    const firstName = this.updateUserForm.value.firstName;
    const lastName = this.updateUserForm.value.lastName;
    const age = this.updateUserForm.value.age;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.identityService.update({ firstName, lastName, age })
      .subscribe(_ => {
        loading.dismiss();
        this.toastService.presentSuccessToast();
      }, () => loading.dismiss());
  }
}