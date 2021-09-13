import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, QueryList, ViewChildren, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IonicSelectableComponent } from 'ionic-selectable';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { DawaAddressModel } from '../../../core/models/dawa/dawa-address.model';
import { FormValidator } from '../../../core/helpers/form-validator';
import { DawaAddressService } from '../../../core/services/dawa-address.service';
import { validations } from '../../../core/constants/validations';
import { RegistrationNumberModel } from '../../../core/models/vehicles/registration-number.model';
import { TripInputModel } from '../../../core/models/trips/trip-input.model';

@Component({
  selector: 'app-trip-form',
  templateUrl: './trip-form.component.html',
  styleUrls: ['./trip-form.component.scss']
})
export class TripFormComponent implements OnInit, OnDestroy, AfterViewInit {
  private addressSub$: Subscription;
  private searchTextDeparture: string;
  private searchDeparturesResult: Array<DawaAddressModel>;
  private searchTextArrival: string;
  private searchArrivalsResult: Array<DawaAddressModel>;

  @Input() isDarkTheme: boolean;
  @Input() isIOS: boolean;
  @Input() registrationNumbers: Array<RegistrationNumberModel>;
  @Input() tripModel: TripInputModel;
  @Input() shouldOpenExtraInfo: boolean;
  @Output() validatedModel: EventEmitter<TripInputModel> = new EventEmitter();
  @ViewChildren('departureAddressSelectable') departureAddressComponent: QueryList<IonicSelectableComponent>;
  @ViewChildren('arrivalAddressSelectable') arrivalAddressComponent: QueryList<IonicSelectableComponent>;

  withinDenmarkDeparture: boolean;
  withinDenmarkArrival: boolean;

  currentYear: number;
  currentDepartureDate: string;
  currentArrivalDate: string;

  vehicleForm: FormGroup;
  showAdditionalInfo: boolean;
  
  constructor(private addressService: DawaAddressService) { }

  ngAfterViewInit(): void {
    if (this.tripModel) {
      if (this.tripModel.departureAddress.x && this.tripModel.departureAddress.y && this.departureAddressComponent.last) {
        this.departureAddressComponent.last.placeholder = this.tripModel.departureAddress.name;
        this.departureAddressComponent.last.searchText = this.tripModel.departureAddress.name;
        this.f.departureAddressWithinDenmark.setValue({ 
          name: this.tripModel.departureAddress.name, 
          id: this.tripModel.departureAddress.name, 
          x: this.tripModel.departureAddress.x, 
          y: this.tripModel.departureAddress.y, 
          notFullAddress: false});
      }

      if (this.tripModel.arrivalAddress.x && this.tripModel.arrivalAddress.y && this.arrivalAddressComponent.last) {
        this.arrivalAddressComponent.last.placeholder = this.tripModel.arrivalAddress.name;
        this.arrivalAddressComponent.last.searchText = this.tripModel.arrivalAddress.name;
        this.f.arrivalAddressWithinDenmark.setValue({ 
          name: this.tripModel.arrivalAddress.name, 
          id: this.tripModel.arrivalAddress.name, 
          x: this.tripModel.arrivalAddress.x, 
          y: this.tripModel.arrivalAddress.y, 
          notFullAddress: false});
      }
    }
  }

  ngOnInit(): void {
    this.setUpForm();
    this.setUpEditForm();
  }

  setUpEditForm(): void {
    if (this.tripModel) {
      this.f.registrationNumber.setValue(this.tripModel.registrationNumber);
      this.f.departureDate.setValue(this.tripModel.departureDate);
      this.f.arrivalDate.setValue(this.tripModel.arrivalDate);
      this.f.distance.setValue(this.tripModel.distance);
      this.f.cost.setValue(this.tripModel.cost);
      this.f.note.setValue(this.tripModel.note);

      if (!this.tripModel.departureAddress.x || !this.tripModel.departureAddress.y) {
        this.withinDenmarkDeparture = false;
        this.f.departureAddress.setValue(this.tripModel.departureAddress.name);
      } else {
        this.withinDenmarkDeparture = true;
        // ngAfterViewInit
      }

      if (!this.tripModel.arrivalAddress.x || !this.tripModel.arrivalAddress.y) {
        this.withinDenmarkArrival = false;
        this.f.arrivalAddress.setValue(this.tripModel.arrivalAddress.name);
      } else {
        this.withinDenmarkArrival = true;
        // ngAfterViewInit
      }
    }
  }

  setUpForm(): void {
    this.withinDenmarkDeparture = true;
    this.withinDenmarkArrival = true;

    const dateNow = new Date();
    this.currentYear = dateNow.getFullYear();

    this.currentDepartureDate = dateNow.toISOString();

    const arrivalDate = dateNow;
    arrivalDate.setMinutes(arrivalDate.getMinutes() + 1);
    this.currentArrivalDate = arrivalDate.toISOString();

    this.showAdditionalInfo = this.shouldOpenExtraInfo;

    this.vehicleForm = new FormGroup({
      registrationNumber: new FormControl(null, {
        validators: [Validators.required]
      }),
      departureDate: new FormControl(this.currentDepartureDate, {
        validators: [Validators.required]
      }),
      arrivalDate: new FormControl(this.currentArrivalDate, {
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
        validators: [Validators.max(validations.trip.COST_MAX_LENGTH)]
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
    if (this.addressSub$) {
      this.addressSub$.unsubscribe();
    }
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

  onSubmit(): void {
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

    const departureDate = new Date(this.vehicleForm.value.departureDate).toISOString();
    const arrivalDate = new Date(this.vehicleForm.value.arrivalDate).toISOString();
    const distance = this.vehicleForm.value.distance;
    const cost = this.vehicleForm.value.cost;
    const note = this.vehicleForm.value.note;
    const registrationNumber = this.vehicleForm.value.registrationNumber;

    this.validatedModel.emit({
      registrationNumber,
      departureDate,
      arrivalDate,
      departureAddress: { name: departureAddress.name, x: departureAddress.x, y: departureAddress.y },
      arrivalAddress: { name: arrivalAddress.name, x: arrivalAddress.x, y: arrivalAddress.y },
      cost,
      distance,
      note});
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

}
