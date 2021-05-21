import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActionSheetController, LoadingController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { loadingOverlays } from '../../core/constants/loadingOverlays';
import { SettingsService } from '../../core/services/settings.service';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss']
})
export class TabsPage implements OnInit, OnDestroy {
  private langSub: Subscription;

  constructor(
    private settingsService: SettingsService, 
    private translateService: TranslateService,
    private loadingCntrl: LoadingController,
    private actionsheetCntrl: ActionSheetController) { }
  
  ngOnInit(): void {
    this.langSub = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));
    this.loadingCntrl.getTop().then(overLay => overLay?.id === loadingOverlays.initialRegistration ? overLay.dismiss() : null);
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }

  async presentActionSheet(): Promise<void> {
    const actionSheet = await this.actionsheetCntrl.create({
      cssClass: 'my-custom-class',
      buttons: [{
        text: this.translateService.instant('Route'),
        icon: 'map-outline',
        handler: () => {
          console.log('Trip clicked');
        }
      }, {
        text: this.translateService.instant('Repair'),
        icon: 'build-outline',
        handler: () => {
          console.log('Repair clicked');
        }
      }, {
        text: this.translateService.instant('Refueling'),
        icon: 'color-fill-outline',
        handler: () => {
          console.log('Refuel clicked');
        }
      }, {
        text: this.translateService.instant('Fee'),
        icon: 'receipt-outline',
        handler: () => {
          console.log('Fee clicked');
        }
      }, {
        text: this.translateService.instant('Cancel'),
        icon: 'close',
        handler: () => {
          console.log('Cancel clicked');
        }
      }]
    });
    await actionSheet.present();
  }
}
