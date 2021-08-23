import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActionSheetController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../core/services/settings.service';
import { AddTripComponent } from './add-trip/add-trip.component';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss']
})
export class TabsPage implements OnInit, OnDestroy {
  private langSub$: Subscription;

  constructor(
    private settingsService: SettingsService, 
    private translateService: TranslateService,
    private actionsheetCntrl: ActionSheetController,
    private modalCntrl: ModalController) {}
  
  ngOnInit(): void {
    this.langSub$ = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));
  }

  ngOnDestroy(): void {
    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }
  }

  async presentActionSheet(): Promise<void> {
    const actionSheet = await this.actionsheetCntrl.create({
      cssClass: 'my-custom-class',
      buttons: [{
        text: this.translateService.instant('Route'),
        icon: 'map-outline',
        handler: async () => {
          await this.presentAddTripModal();
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
        text: this.translateService.instant('Cancel'),
        icon: 'close',
        handler: () => {
          console.log('Cancel clicked');
        }
      }]
    });
    await actionSheet.present();
  }

  private async presentAddTripModal(): Promise<void> {
    const modal = await this.modalCntrl.create({
      component: AddTripComponent
    });

    return await modal.present();
  }
}
