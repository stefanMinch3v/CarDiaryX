import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoadingController, Platform } from '@ionic/angular';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../../core/services/settings.service';
import { TripsService } from '../../../core/services/trips.service';
import { ToastService } from '../../../core/services/toast.service';
import { RegistrationNumberModel } from '../../../core/models/vehicles/registration-number.model';
import { TripInputModel } from '../../../core/models/trips/trip-input.model';

@Component({
  selector: 'app-trip-edit',
  templateUrl: './trip-edit.component.html',
  styleUrls: ['./trip-edit.component.scss'],
})
export class TripEditComponent implements OnInit, OnDestroy {
  private themeSub$: Subscription;
  private tripId: number;
  
  tripModel: TripInputModel;
  registrationNumbers: Array<RegistrationNumberModel>;
  isDarkTheme: boolean;
  isIOS: boolean;
  
  constructor(
    private platform: Platform,
    private location: Location, 
    private loadingCntrl: LoadingController,
    private settingsService: SettingsService,
    private tripsService: TripsService,
    private toastService: ToastService,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');
    this.themeSub$ = this.settingsService.currentTheme.subscribe(t => this.isDarkTheme = t);
    const extraData = this.activatedRoute.snapshot.data?.extraData;
    this.registrationNumbers = extraData?.registrationNumbers;
    this.tripModel = extraData?.tripModel;
    this.tripId = extraData?.tripId;
  }

  ngOnDestroy(): void {
    if (this.themeSub$) {
      this.themeSub$.unsubscribe();
    }
  }

  onNavigateBack(): void {
    return this.location.back();
  }

  async onSubmit(tripModel: TripInputModel): Promise<void> {
    if (!tripModel) {
      return;
    }

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.tripsService.update(this.tripId, {
      registrationNumber: tripModel.registrationNumber,
      departureDate: tripModel.departureDate,
      arrivalDate: tripModel.arrivalDate,
      departureAddress: tripModel.departureAddress,
      arrivalAddress: tripModel.arrivalAddress,
      cost: tripModel.cost,
      distance: tripModel.distance,
      note: tripModel.note
    })
    .subscribe(_ => {
        this.toastService.presentSuccessToast();
        this.onNavigateBack();
      },
      () => loading.dismiss(),
      () => loading.dismiss()
    );
  }

}
