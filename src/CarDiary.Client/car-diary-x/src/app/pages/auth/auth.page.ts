import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController, MenuController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { RegisterComponent } from './register/register.component';
import { SettingsService } from '../../core/services/settings.service';
import { AuthService } from '../../core/services/auth.service';
import { IdentityService } from '../../core/services/identity.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.page.html',
  styleUrls: ['./auth.page.scss'],
})
export class AuthPage implements OnInit, OnDestroy {
  private langSub$: Subscription;
  private loginSub$: Subscription;

  constructor(
    private identityService: IdentityService, 
    private authService: AuthService,
    private loadingCntrl: LoadingController,
    private router: Router,
    private modalCntrl: ModalController,
    private settingsService: SettingsService,
    private translateService: TranslateService,
    private menuCntrl: MenuController) { }
  
  ngOnInit(): void {
    this.menuCntrl.enable(false);
    this.langSub$ = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));
  }

  ngOnDestroy(): void {
    this.menuCntrl.enable(true);

    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }

    if (this.loginSub$) {
      this.loginSub$.unsubscribe();
    }
  }

  ionViewWillEnter(): void {
    this.menuCntrl.enable(false);
  }

  ionViewWillLeave(): void {
    this.menuCntrl.enable(true);

    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }

    if (this.loginSub$) {
      this.loginSub$.unsubscribe();
    }
  }

  async presentModal(): Promise<void> {
    const modal = await this.modalCntrl.create({
      component: RegisterComponent
    });

    return await modal.present();
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (!form.valid) {
      return;
    }

    const email = form.value.email;
    const password = form.value.password;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.loginSub$ = this.identityService
      .login({ email, password })
      .subscribe(response => {
        const token = response?.token;
        const expiration = response?.expiration;

        this.authService.authenticateUser(token, expiration);
        this.router.navigate(['tabs']);
      }, () => loading.dismiss(), () => loading.dismiss());
  }

  openUrl(): void {
    window.open('https://google.com', '_system');
  }
}
