import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoadingController, Platform } from '@ionic/angular';
import { ToastService } from '../../../core/services/toast.service';
import { validations } from '../../../core/constants/validations';
import { VehicleService } from '../../../core/services/vehicle.service';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.scss'],
})
export class VehicleFormComponent implements OnInit {
  isIOS: boolean;
  isLoading: boolean;
  searchRegistrationNumberForm: FormGroup;
  registrationNumberMaxLength = validations.vehicle.REGISTRATION_NUMBER_LENGTH;

  constructor(
    private location: Location, 
    private platform: Platform, 
    private vehicleService: VehicleService,
    private loadingCntrl: LoadingController,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');
    this.isLoading = true;
    this.searchRegistrationNumberForm = new FormGroup({
      registrationNumber: new FormControl(null, {
        validators: [
          Validators.required, 
          Validators.maxLength(validations.vehicle.REGISTRATION_NUMBER_LENGTH),
          Validators.minLength(validations.vehicle.REGISTRATION_NUMBER_LENGTH)]
      })
    });
  }

  onNavigateBack(): void {
    return this.location.back();
  }

  async onSubmit(): Promise<void> {
    if (!this.searchRegistrationNumberForm.valid) {
      return;
    }

    const registrationNumber = this.searchRegistrationNumberForm.value.registrationNumber;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.vehicleService.addToUser(String(registrationNumber).toUpperCase())
      .subscribe(_ => {
        // load get all vehicles so the subscription works when navigates back to base page
        this.toastService.presentSuccessToast();
        this.onNavigateBack();
      }, () => loading.dismiss(), () => loading.dismiss());
  }
}
