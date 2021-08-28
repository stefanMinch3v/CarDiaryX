import { Location } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoadingController, ModalController, Platform } from '@ionic/angular';
import { IonicSelectableComponent } from 'ionic-selectable';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { DawaAddressModel } from '../../../core/models/dawa/dawa-address.model';
import { FormValidator } from '../../../core/helpers/form-validator';
import { DawaAddressService } from '../../../core/services/dawa-address.service';
import { validations } from '../../../core/constants/validations';
import { SettingsService } from '../../../core/services/settings.service';
import { TripsService } from '../../../core/services/trips.service';
import { ToastService } from '../../../core/services/toast.service';
import { VehicleService } from '../../../core/services/vehicle.service';
import { RegistrationNumberModel } from '../../../core/models/vehicles/registration-number.model';

@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss']
})
export class AddTripComponent implements OnInit, OnDestroy {
  private userRegistrationNumbersSub$: Subscription;
  private addressSub$: Subscription;
  private addTripSub$: Subscription;
  private themeSub$: Subscription;
  private searchTextDeparture: string;
  private searchDeparturesResult: Array<DawaAddressModel>;
  private searchTextArrival: string;
  private searchArrivalsResult: Array<DawaAddressModel>;

  @Input() registrationNumbers: Array<RegistrationNumberModel>;

  withinDenmarkDeparture: boolean;
  withinDenmarkArrival: boolean;

  currentYear: number;
  currentDate: string;

  vehicleForm: FormGroup;

  isDarkTheme: boolean;
  isIOS: boolean;
  isLoading: boolean;
  showAdditionalInfo: boolean;
  
  constructor(
    private platform: Platform,
    private modalCntrl: ModalController,
    private addressService: DawaAddressService,
    private location: Location, 
    private loadingCntrl: LoadingController,
    private settingsService: SettingsService,
    private tripsService: TripsService,
    private toastService: ToastService,
    private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');
    this.withinDenmarkDeparture = true;
    this.withinDenmarkArrival = true;
    this.currentYear = new Date().getFullYear();
    this.currentDate = new Date().toISOString();
    this.themeSub$ = this.settingsService.currentTheme.subscribe(t => this.isDarkTheme = t);
    
    if (!this.registrationNumbers) {
      this.userRegistrationNumbersSub$ = this.vehicleService.fetchAllRegistrationNumbers()
        .subscribe(numbers => this.registrationNumbers = numbers);
    }

    this.vehicleForm = new FormGroup({
      registrationNumber: new FormControl(null, {
        validators: [Validators.required]
      }),
      departureDate: new FormControl(this.currentDate, {
        validators: [Validators.required]
      }),
      arrivalDate: new FormControl(this.currentDate, {
        validators: [Validators.required]
      }),
      departureAddressWithinDenmark: new FormControl(null),
      arrivalAddressWithinDenmark: new FormControl(null),
      departureAddress: new FormControl(null, {
        validators: [Validators.minLength(validations.trip.ADDRESS_MIN_LENGTH), Validators.maxLength(validations.trip.ADDRESS_MAX_LENGTH)]
      }),
      arrivalAddress: new FormControl(null, {
        validators: [Validators.minLength(validations.trip.ADDRESS_MIN_LENGTH), Validators.maxLength(validations.trip.ADDRESS_MAX_LENGTH)]
      }),
      distance: new FormControl(null, {
        validators: [Validators.max(validations.trip.DISTANCE_MAX_LENGTH)]
      }),
      cost: new FormControl(null, {
        validators: [Validators.maxLength(validations.trip.COST_MAX_LENGTH)]
      }),
      note: new FormControl(null, {
        validators: [Validators.maxLength(validations.trip.NOTE_MAX_LENGTH)]
      })
    }, {
      validators: [
        FormValidator.matchValidAddressInDenmark('departureAddressWithinDenmark'), 
        FormValidator.matchValidAddressInDenmark('arrivalAddressWithinDenmark'),
        FormValidator.matchDepartureArrivalDates
      ]
    });
  }

  get f() { return this.vehicleForm.controls; }

  ngOnDestroy(): void {
    this.removeSubscriptions();
  }

  isFormValid(): boolean {
    const departureWithinDenmark = this.withinDenmarkDeparture && 
      this.vehicleForm.value.departureAddressWithinDenmark && 
      this.f.departureAddressWithinDenmark.valid &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      this.f.registrationNumber.valid;

    const departureOutsideDenmark = !this.withinDenmarkDeparture && 
      this.f.departureAddress.valid &&
      this.vehicleForm.value.departureAddress &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      this.f.registrationNumber.valid;

    const arrivalWithinDenmark = this.withinDenmarkArrival && 
      this.f.arrivalAddressWithinDenmark.valid &&
      this.vehicleForm.value.arrivalAddressWithinDenmark &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      this.f.registrationNumber.valid;

    const arrivalOutsideDenmark = !this.withinDenmarkArrival && 
      this.f.arrivalAddress.valid && 
      this.vehicleForm.value.arrivalAddress &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      this.f.registrationNumber.valid;

    return departureWithinDenmark && arrivalWithinDenmark ||
      departureWithinDenmark && arrivalOutsideDenmark ||
      departureOutsideDenmark && arrivalWithinDenmark ||
      departureOutsideDenmark && arrivalOutsideDenmark;
  }

  isValidForAllRequeiredExceptRegNumber(): boolean {
    const departureWithinDenmark = this.withinDenmarkDeparture && 
      this.vehicleForm.value.departureAddressWithinDenmark && 
      this.f.departureAddressWithinDenmark.valid &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      !this.f.registrationNumber.valid;

    const departureOutsideDenmark = !this.withinDenmarkDeparture && 
      this.f.departureAddress.valid &&
      this.vehicleForm.value.departureAddress &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      !this.f.registrationNumber.valid;

    const arrivalWithinDenmark = this.withinDenmarkArrival && 
      this.f.arrivalAddressWithinDenmark.valid &&
      this.vehicleForm.value.arrivalAddressWithinDenmark &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      !this.f.registrationNumber.valid;

    const arrivalOutsideDenmark = !this.withinDenmarkArrival && 
      this.f.arrivalAddress.valid && 
      this.vehicleForm.value.arrivalAddress &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid &&
      !this.f.registrationNumber.valid;

    return departureWithinDenmark && arrivalWithinDenmark ||
      departureWithinDenmark && arrivalOutsideDenmark ||
      departureOutsideDenmark && arrivalWithinDenmark ||
      departureOutsideDenmark && arrivalOutsideDenmark;
  }

  onNavigateBack(): void {
    return this.location.back();
  }

  onDismissModal(): void {
    this.modalCntrl.dismiss();
  }

  async onSubmit(): Promise<void> {
    if (!this.vehicleForm.value || !this.vehicleForm.valid) {
      return;
    }

    let departureAddress: DawaAddressModel;
    let arrivalAddress: DawaAddressModel;
  
    if (this.withinDenmarkDeparture) {
      departureAddress = this.vehicleForm.value.departureAddressWithinDenmark;
    } else {
      departureAddress = { id: null, name: this.vehicleForm.value.departureAddress, x: null, y: null, notFullAddress: true };
    }

    if (this.withinDenmarkArrival) {
      arrivalAddress = this.vehicleForm.value.arrivalAddressWithinDenmark;
    } else {
      arrivalAddress = { id: null, name: this.vehicleForm.value.arrivalAddress, x: null, y: null, notFullAddress: true };
    }

    const departureDate = this.vehicleForm.value.departureDate;
    const arrivalDate = this.vehicleForm.value.arrivalDate;
    const distance = this.vehicleForm.value.distance;
    const cost = this.vehicleForm.value.cost;
    const note = this.vehicleForm.value.note;
    const registrationNumber = this.vehicleForm.value.registrationNumber;

    const loading = await this.loadingCntrl.create({ keyboardClose: true });
    await loading.present();

    this.addTripSub$ = this.tripsService.add({
      registrationNumber,
      departureDate: new Date(departureDate).toISOString(),
      arrivalDate: new Date(arrivalDate).toISOString(),
      departureAddress: { name: departureAddress.name, x: departureAddress.x, y: departureAddress.y },
      arrivalAddress: { name: arrivalAddress.name, x: arrivalAddress.x, y: arrivalAddress.y },
      cost,
      distance,
      note})
      .subscribe(_ => {
          this.onDismissModal();
          this.toastService.presentSuccessToast();
        },
        () => loading.dismiss(),
        () => loading.dismiss()
      );
  }

  onAddressSearch(event: { component: IonicSelectableComponent, text: string }, isDeparture: boolean = true): void {
    const text = event.text
      .trim()
      .toLowerCase();

    event.component.startSearch();

    this.closeSubscriptionsForSearch();

    if (!text) {
      this.closeSubscriptionsForSearch();

      event.component.items = [];
      event.component.endSearch();

      if (isDeparture) {
        this.searchDeparturesResult = [];
        this.searchTextDeparture = text;
      } else {
        this.searchArrivalsResult = [];
        this.searchTextArrival = text;
      }

      return;
    } else if (isDeparture && this.searchTextDeparture === text && this.searchDeparturesResult) {
      this.closeSubscriptionsForSearch();
      event.component.endSearch();
      return;
    } else if (!isDeparture && this.searchTextArrival === text && this.searchArrivalsResult) {
      this.closeSubscriptionsForSearch();
      event.component.endSearch();
      return;
    }

    if (isDeparture) {
      this.searchTextDeparture = text;
    } else {
      this.searchTextArrival = text;
    }

    this.addressSub$ = this.addressService.get(text)
      .pipe(map(this.parseAddress))
      .subscribe(result => {
        if (this.addressSub$.closed) {
          return;
        }

        event.component.items = result;
        event.component.endSearch();

        if (isDeparture) {
          this.searchDeparturesResult = event.component.items;
        } else {
          this.searchArrivalsResult = event.component.items;
        }
      });
  }

  parseAddress(address: any): Array<DawaAddressModel> {
    const arr: Array<DawaAddressModel> = [];

    const notFullAddress = address.some(a => !a.data?.id);

    if (notFullAddress) {
      let fakeIdCounter = 0;

      address
        .map(a => arr.push({ 
          id: String(++fakeIdCounter),
          name: a.forslagstekst?.trim(), 
          x: null,
          y: null,
          notFullAddress: true
        }));
    } else {
      address
        .map(a => arr.push({ 
          id: a.data?.id, 
          name: a.forslagstekst?.trim(), 
          x: a.data?.x,
          y: a.data?.y,
          notFullAddress: false
        }));
    }

    return arr;
  }

  onDepartureChange(): void {
    this.withinDenmarkDeparture = !this.withinDenmarkDeparture;
  }

  onArrivalChange(): void {
    this.withinDenmarkArrival = !this.withinDenmarkArrival;
  }

  toggleAdditionalInfoSection(): void {
    this.showAdditionalInfo = !this.showAdditionalInfo;
  }

  private closeSubscriptionsForSearch(): void {
    if (this.addressSub$) {
      this.addressSub$.unsubscribe();
    }
  }

  private removeSubscriptions(): void {
    if (this.themeSub$) {
      this.themeSub$.unsubscribe();
    }

    if (this.addressSub$) {
      this.addressSub$.unsubscribe();
    }

    if (this.addTripSub$) {
      this.addTripSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
    }
  }
}
