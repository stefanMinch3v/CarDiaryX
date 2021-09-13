import { Component, OnInit } from '@angular/core';
import { ActionSheetController } from '@ionic/angular';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { VehicleService } from '../../core/services/vehicle.service';
import { SettingsService } from '../../core/services/settings.service';
import { RegistrationNumberModel } from '../../core/models/vehicles/registration-number.model';
import { DataService } from '../../core/services/service-resolvers/data.service';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss']
})
export class TabsPage implements OnInit {
  private regNumbersSub$: Subscription;
  private langSub$: Subscription;
  private regNumbersStreamSub$: Subscription;
  private registrationNumbers: Array<RegistrationNumberModel>;

  constructor(
    private settingsService: SettingsService, 
    private translateService: TranslateService,
    private actionsheetCntrl: ActionSheetController,
    private vehicleService: VehicleService,
    private router: Router,
    private tripsDataService: DataService,
    private activatedRoute: ActivatedRoute) {}
  
  ngOnInit(): void {
    this.langSub$ = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));

    // temp, TODO: remove all code below, its here for testing purposes! (on refresh page to force get instead of login/logout)
    if (!this.regNumbersSub$) {
      this.regNumbersSub$ = this.vehicleService.fetchAllRegistrationNumbers().subscribe();
    }
  }

  ionViewWillEnter(): void {
    const isComingFromAuthPage = this.activatedRoute.snapshot.data?.extraData?.isComingFromAuthPage;

    if (isComingFromAuthPage) {
      this.regNumbersSub$ = this.vehicleService.fetchAllRegistrationNumbers()
        .subscribe(numbers => {
          this.activatedRoute.snapshot.data.extraData.isComingFromAuthPage = false;
          this.navigateToAddVehicleIfEmptyRegNumbers(numbers);
        });
      return;
    }

    this.regNumbersStreamSub$ = this.vehicleService.registrationNumbers
      .subscribe(numbers => this.navigateToAddVehicleIfEmptyRegNumbers(numbers));
  }

  ionViewWillLeave(): void {
    if (this.langSub$) {
      this.langSub$.unsubscribe();
    }

    if (this.regNumbersStreamSub$) {
      this.regNumbersStreamSub$.unsubscribe();
    }

    if (this.regNumbersSub$) {
      this.regNumbersSub$.unsubscribe();
    }
  }

  async presentActionSheet(): Promise<void> {
    const actionSheet = await this.actionsheetCntrl.create({
      cssClass: 'my-custom-class',
      buttons: [{
        text: this.translateService.instant('Route'),
        icon: 'map-outline',
        handler: () => {
          this.tripsDataService.setData = { registrationNumbers: this.registrationNumbers };
          this.router.navigate(['trips', 'trip-add']);
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

  private navigateToAddVehicleIfEmptyRegNumbers(numbers: Array<RegistrationNumberModel>): void {
    if (numbers && numbers.length === 0) {
      this.router.navigate(['garage', 'add-vehicle']);
    }

    this.registrationNumbers = numbers;
  }
}
