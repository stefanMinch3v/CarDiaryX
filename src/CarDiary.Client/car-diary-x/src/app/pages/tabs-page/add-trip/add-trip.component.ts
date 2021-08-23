import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoadingController, ModalController, Platform } from '@ionic/angular';
import { IonicSelectableComponent } from 'ionic-selectable';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { DawaAddressModel } from '../../../core/models/dawa/dawa-address.model';
import { FormValidator } from '../../../core/helpers/form-validator';
import { DawaAddressService } from '../../../core/services/dawa-address.service';

@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss']
})
export class AddTripComponent implements OnInit {
  private addressSub$: Subscription;
  private searchTextDeparture: string;
  private searchDeparturesResult: Array<DawaAddressModel>;
  private searchTextArrival: string;
  private searchArrivalsResult: Array<DawaAddressModel>;
  
  withinDenmarkDeparture: boolean;
  withinDenmarkArrival: boolean;

  currentYear: number;
  currentDate: string;

  vehicleForm: FormGroup;

  isIOS: boolean;
  isLoading: boolean;
  
  constructor(
    private platform: Platform,
    private modalCntrl: ModalController,
    private addressService: DawaAddressService,
    private location: Location, 
    private loadingCntrl: LoadingController) { }

  ngOnInit(): void {
    this.isIOS = this.platform.is('ios');
    this.withinDenmarkDeparture = true;
    this.withinDenmarkArrival = true;
    this.currentYear = new Date().getFullYear();
    this.currentDate = new Date().toISOString();

    this.vehicleForm = new FormGroup({
      departureDate: new FormControl(this.currentDate, {
        validators: [Validators.required]
      }),
      arrivalDate: new FormControl(this.currentDate, {
        validators: [Validators.required]
      }),
      departureAddressWithinDenmark: new FormControl(null),
      arrivalAddressWithinDenmark: new FormControl(null),
      departureAddress: new FormControl(null, {
        validators: [Validators.minLength(2)]
      }),
      arrivalAddress: new FormControl(null, {
        validators: [Validators.minLength(2)]
      }),
      distance: new FormControl(null, {
        validators: [Validators.max(10_000_000)]
      }),
      cost: new FormControl(null, {
        validators: [Validators.maxLength(15)]
      }),
      note: new FormControl(null, {
        validators: [Validators.maxLength(250)]
      })
    }, {
      validators: [
        FormValidator.matchValidAddressInDenmark('departureAddressWithinDenmark'), 
        FormValidator.matchValidAddressInDenmark('arrivalAddressWithinDenmark')
      ]
    });
  }

  get f() { return this.vehicleForm.controls; }

  isFormValid(): boolean {
    const departureWithinDenmark = this.withinDenmarkDeparture && 
      this.vehicleForm.value.departureAddressWithinDenmark && 
      this.f.departureAddressWithinDenmark.valid &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid;

    const departureOutsideDenmark = !this.withinDenmarkDeparture && 
      this.f.departureAddress.valid &&
      this.vehicleForm.value.departureAddress &&
      this.f.departureDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid;

    const arrivalWithinDenmark = this.withinDenmarkArrival && 
      this.f.arrivalAddressWithinDenmark.valid &&
      this.vehicleForm.value.arrivalAddressWithinDenmark &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid;

    const arrivalOutsideDenmark = !this.withinDenmarkArrival && 
      this.f.arrivalAddress.valid && 
      this.vehicleForm.value.arrivalAddress &&
      this.f.arrivalDate.valid &&
      this.f.distance.valid &&
      this.f.cost.valid &&
      this.f.note.valid;

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
      departureAddress = { id: null, name: this.vehicleForm.value.departureAddress, x: null, y: null };
    }

    if (this.withinDenmarkArrival) {
      arrivalAddress = this.vehicleForm.value.arrivalAddressWithinDenmark;
    } else {
      arrivalAddress = { id: null, name: this.vehicleForm.value.arrivalAddress, x: null, y: null };
    }

    const departureDate = this.vehicleForm.value.departureDate;
    const arrivalDate = this.vehicleForm.value.arrivalDate;
    const distance = this.vehicleForm.value.distance;
    const cost = this.vehicleForm.value.cost;
    const note = this.vehicleForm.value.note;

    console.log(departureAddress);
    console.log(arrivalAddress);
    console.log(departureDate);
    console.log(arrivalDate);
    console.log(distance);
    console.log(cost);
    console.log(note);

    //const loading = await this.loadingCntrl.create({ keyboardClose: true });
    //await loading.present();

    // this.registerSub$ = this.identityService.register({ firstName, lastName, email, password })
    //   .pipe(
    //     switchMap(_ => this.identityService.login({ email, password })))
    //   .subscribe(response => {
    //     const token = response?.token;
    //     const expiration = response?.expiration;

    //     this.authService.authenticateUser(token, expiration);
    //     this.router.navigate(['tabs']);

    //     this.onDismissModal();
    //   }, () => loading.dismiss(), () => loading.dismiss());
  }

  onAddressSearch(event: { component: IonicSelectableComponent, text: string }, isDeparture: boolean = true): void {
    const text = event.text
      .trim()
      .toLowerCase();

    event.component.startSearch();

    this.closeSubscriptions();

    if (!text) {
      this.closeSubscriptions();

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
      this.closeSubscriptions();
      event.component.endSearch();
      return;
    } else if (!isDeparture && this.searchTextArrival === text && this.searchArrivalsResult) {
      this.closeSubscriptions();
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

    address
      .map(a => arr.push({ 
        id: a.data?.id, 
        name: a.forslagstekst.trim(), 
        x: a.data?.x,
        y: a.data?.y 
      }));

    return arr;
  }

  onDepartureChange(): void {
    this.withinDenmarkDeparture = !this.withinDenmarkDeparture;
  }

  onArrivalChange(): void {
    this.withinDenmarkArrival = !this.withinDenmarkArrival;
  }

  private closeSubscriptions(): void {
    if (this.addressSub$) {
      this.addressSub$.unsubscribe();
    }
  }
}
