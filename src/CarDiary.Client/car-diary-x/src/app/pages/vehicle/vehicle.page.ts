import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { VehicleService } from '../../core/services/vehicle.service';
import { SettingsService } from '../../core/services/settings.service';
import { RegistrationNumberModel } from '../../core/models/vehicles/registration-number.model';
import { ActionSheetController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.page.html',
  styleUrls: ['./vehicle.page.scss'],
})
export class VehiclePage implements OnInit, OnDestroy {
  private vehicleFilterSub$: Subscription;
  private userRegistrationNumbersSub$: Subscription;
  registrationNumbers: Array<RegistrationNumberModel>;
  showList: boolean;
  isLoading = true;

  constructor(
    private location: Location, 
    private settingsService: SettingsService,
    private router: Router,
    private vehicleService: VehicleService,
    private actionsheetCntrl: ActionSheetController,
    private translateService: TranslateService) { }

  ngOnInit(): void {
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
  }

  ionViewWillLeave(): void {
    if (this.vehicleFilterSub$) {
      this.vehicleFilterSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
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
    this.router.navigate(['vehicles', 'vehicle-form']);
  }

  prettifyShortDescription(shortDescription: string): string {
    return shortDescription
      ?.split('^')
      .filter(d => d != '')
      .join(' ');
  }

  onNavigateToDetails(registrationNumber: string) {
    console.log('here');
    this.router.navigate(['vehicles', 'vehicle-details', registrationNumber]);
  }

  isMotorCycle(registrationNumber: string): boolean {
    return registrationNumber.length === 6;
  }

  async presentActionSheet(): Promise<void> {
    const actionSheet = await this.actionsheetCntrl.create({
      cssClass: 'my-custom-class',
      buttons: [{
        text: this.translateService.instant('Open'),
        icon: 'eye-outline',
        handler: () => {
          console.log('Trip clicked');
        }
      }, {
        text: this.translateService.instant('Delete'),
        icon: 'trash-outline',
        handler: () => {
          console.log('Repair clicked');
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
