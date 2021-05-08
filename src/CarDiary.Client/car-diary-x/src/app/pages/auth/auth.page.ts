import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingController, ModalController } from '@ionic/angular';
import { RegisterComponent } from 'src/app/components/register/register.component';
import { AuthService } from '../../core/services/auth.service';
import { IdentityService } from '../../core/services/identity.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.page.html',
  styleUrls: ['./auth.page.scss'],
})
export class AuthPage implements OnInit {
  isLoginMode = true;

  constructor(
    private identityService: IdentityService, 
    private authService: AuthService,
    private loadingCntrl: LoadingController,
    private router: Router,
    private modalCntrl: ModalController) { }

  ngOnInit() {
  }

  onLogin(): void { 
  }

  onSwitchAuthMode(): void {
    this.isLoginMode = !this.isLoginMode;
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

    if (this.isLoginMode) {
      const loading = await this.loadingCntrl.create({ keyboardClose: true });
      await loading.present();

      this.identityService
        .login({ email, password })
        .subscribe(async response => {
          const token = response.token;
          const expiration = response.expiration;

          this.authService.authenticateUser(token, expiration);
          this.router.navigate(['tabs']);
          await loading.dismiss();
        });
    } else {
      // TODO
      // send register
    }
  }
}
