import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActionSheetController, AlertController, LoadingController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { VehicleService } from '../../core/services/vehicle.service';
import { SettingsService } from '../../core/services/settings.service';
import { RegistrationNumberModel } from '../../core/models/vehicles/registration-number.model';

@Component({
  selector: 'app-garage',
  templateUrl: './garage.page.html',
  styleUrls: ['./garage.page.scss'],
})
export class GaragePage implements OnInit, OnDestroy {
  private vehicleFilterSub$: Subscription;
  private userRegistrationNumbersSub$: Subscription;
  private vehicleRemoveFromUserSub$: Subscription;
  registrationNumbers: Array<RegistrationNumberModel>;
  showList: boolean;
  isLoading: boolean;;

  constructor(
    private location: Location, 
    private settingsService: SettingsService,
    private router: Router,
    private vehicleService: VehicleService,
    private actionsheetCntrl: ActionSheetController,
    private translateService: TranslateService,
    private alertCntrl: AlertController,
    private loadingCntrl: LoadingController) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.vehicleFilterSub$ = this.settingsService.currentVehicleFilter.subscribe(value => this.showList = value);
  }

  ionViewWillEnter(): void {
    this.userRegistrationNumbersSub$ = this.vehicleService.getAllRegistrationNumbers()
      .subscribe(
        numbers => this.registrationNumbers = numbers,
        () => this.isLoading = false, 
        () => this.isLoading = false);
  }

  ngOnDestroy(): void {
    if (this.vehicleFilterSub$) {
      this.vehicleFilterSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
    }

    if (this.vehicleRemoveFromUserSub$) {
      this.vehicleRemoveFromUserSub$.unsubscribe();
    }
  }

  ionViewWillLeave(): void {
    if (this.vehicleFilterSub$) {
      this.vehicleFilterSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
    }

    if (this.vehicleRemoveFromUserSub$) {
      this.vehicleRemoveFromUserSub$.unsubscribe();
    }
  }

  onNavigateBack(): void {
    return this.location.back();
  }

  toggleFilter(): void {
    this.showList = !this.showList;
    this.settingsService.setCurrentVehicleFilter = this.showList;
  }

  onNavigateToForm(): void {
    this.router.navigate(['garage', 'add-vehicle']);
  }

  prettifyShortDescription(shortDescription: string): string {
    return shortDescription
      ?.split('^')
      .filter(d => d != '')
      .join(' ');
  }

  onNavigateToDetails(registrationNumber: string) {
    this.router.navigate(['garage', 'vehicle-details', registrationNumber]);
  }

  async onDeleteVehicle(registrationNumber: string): Promise<void> {
    const alert = await this.alertCntrl.create({
      cssClass: 'delete-vehicle-alert',
      header: this.translateService.instant('Vehicle will be permanently deleted!'),
      buttons: [
        {
          text: this.translateService.instant('Cancel'),
          role: 'cancel'
        }, {
          text: this.translateService.instant('Confirm'),
          role: 'delete',
          handler: async () => {
            const loading = await this.loadingCntrl.create({ keyboardClose: true });
            await loading.present();

            this.vehicleRemoveFromUserSub$ = this.vehicleService.removeFromUser(registrationNumber)
              .subscribe(_ => 
                this.registrationNumbers = this.registrationNumbers.filter(rn => rn.number !== registrationNumber),
                () => loading.dismiss(), 
                () => loading.dismiss());
          }
        }
      ]
    });

    await alert.present();
  }

  isMotorCycle(registrationNumber: string): boolean {
    return registrationNumber.length === 6;
  }

  async presentActionSheet(registrationNumber: string): Promise<void> {
    if (!registrationNumber) {
      return;
    }

    const actionSheet = await this.actionsheetCntrl.create({
      buttons: [{
        text: this.translateService.instant('Open'),
        icon: 'eye-outline',
        handler: () => {
          this.onNavigateToDetails(registrationNumber);
        }
      }, {
        text: this.translateService.instant('Delete'),
        icon: 'trash-outline',
        handler: () => {
          this.onDeleteVehicle(registrationNumber);
        }
      }, {
        text: this.translateService.instant('Cancel'),
        icon: 'close',
        handler: () => { }
      }]
    });
    await actionSheet.present();
  }


}
