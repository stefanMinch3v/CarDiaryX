import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActionSheetController, ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { VehicleService } from '../../core/services/vehicle.service';
import { SettingsService } from '../../core/services/settings.service';
import { AddTripComponent } from './add-trip/add-trip.component';
import { RegistrationNumberModel } from '../../core/models/vehicles/registration-number.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss']
})
export class TabsPage implements OnInit, OnDestroy {
  private langSub$: Subscription;
  private userRegistrationNumbersSub$: Subscription;
  private registrationNumbers: Array<RegistrationNumberModel>;

  constructor(
    private settingsService: SettingsService, 
    private translateService: TranslateService,
    private actionsheetCntrl: ActionSheetController,
    private modalCntrl: ModalController,
    private vehicleService: VehicleService,
    private router: Router) {}
  
  ngOnInit(): void {
    this.langSub$ = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));
    this.vehicleService.fetchAllRegistrationNumbers().subscribe(numbers => {
      if (numbers && numbers.length === 0) {
        this.router.navigate(['garage', 'add-vehicle']);
      }

      this.registrationNumbers = numbers;
    });
  }

  ionViewWillEnter(): void {
    this.userRegistrationNumbersSub$ = this.vehicleService.registrationNumbers
      .subscribe(numbers => {
        if (numbers && numbers.length === 0) {
          this.router.navigate(['garage', 'add-vehicle']);
        }

        this.registrationNumbers = numbers;
      });
  }

  ngOnDestroy(): void {
    this.removeSubscriptions();
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
      component: AddTripComponent,
      componentProps: { registrationNumbers: this.registrationNumbers }
    });

    return await modal.present();
  }

  private removeSubscriptions(): void {
    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
    }
  }
}
