import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Network, NetworkStatus, PluginListenerHandle } from '@capacitor/core';
import { MenuController, ToastController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { IdentityService } from './core/services/identity.service';
import { SettingsService } from './core/services/settings.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {
  private themeSub$: Subscription;
  private networkListener: PluginListenerHandle;
  private networkStatus: NetworkStatus;
  private toast: HTMLIonToastElement;

  appPages = [
    {
      title: 'My Garage',
      url: '/garage',
      icon: 'calendar'
    },
    {
      title: 'Taxes/Fees',
      url: '/app/tabs/speakers',
      icon: 'people'
    },
    {
      title: 'Trips',
      url: '/app/tabs/map',
      icon: 'map'
    },
    {
      title: 'Refuels',
      url: '/app/tabs/about',
      icon: 'information-circle'
    },
    {
      title: 'Repairs',
      url: '/app/tabs/about',
      icon: 'information-circle'
    },
    {
      title: 'Download PDF',
      url: '/app/tabs/about',
      icon: 'information-circle'
    }
  ];
  
  isDarkTheme: boolean;

  constructor(
    private settingsService: SettingsService,
    private identityService: IdentityService,
    private router: Router,
    private toastCntrl: ToastController,
    private translateService: TranslateService,
    private menuCntrl: MenuController) { }

  ngOnInit(): void {
    this.menuCntrl.swipeGesture(false);
    this.themeSub$ = this.settingsService.currentTheme.subscribe(isDarkTheme => this.isDarkTheme = isDarkTheme);
  }

  async ngAfterViewInit(): Promise<void> {
    this.networkListener = Network.addListener('networkStatusChange', async status => {
      this.networkStatus = status;

      // TODO: test on android/ios
      if (!this.networkStatus.connected) {
        await this.initializeToast();
        await this.presentToast();
      } else {
        await this.dismissToast();
      }
    });

    this.networkStatus = await Network.getStatus();
    console.log(this.networkStatus);

    // test on browser
    // if (this.networkStatus.connected) {
    //   setInterval(async () => {
    //     await this.initializeToast();
    //     await this.presentToast();
    //     setTimeout(async () => {
    //       await this.dismissToast();
    //     }, 2000)
    //   }, 6000)
    // }
  }

  ngOnDestroy(): void {
    if (this.themeSub$) {
      this.themeSub$.unsubscribe();
    }

    this.networkListener.remove();
  }

  ionViewWillLeave(): void {
    if (this.themeSub$) {
      this.themeSub$.unsubscribe();
    }

    this.networkListener.remove();
  }

  onLogout(): void {
    this.identityService.logout();
    this.router.navigate(['/auth']);
  }

  private async initializeToast(): Promise<void> {
    const key = 'No internet connection!';
    await this.translateService.get(key).toPromise();

    this.toast = await this.toastCntrl.create({
      message: this.translateService.instant(key),
      position: 'bottom',
      duration: 0,
      animated: true,
      color: 'warning',
      keyboardClose: true,
    });
  }

  private async presentToast(): Promise<void> {
    if (this.toast) {
      await this.toast.present();
    }
  }

  private async dismissToast(): Promise<void> {
    if (this.toast) {
      await this.toast.dismiss();
    }
  }
}
