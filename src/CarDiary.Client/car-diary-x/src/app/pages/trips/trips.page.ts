import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActionSheetController, AlertController, LoadingController, Platform } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { RegistrationNumberModel } from '../../core/models/vehicles/registration-number.model';
import { VehicleService } from '../../core/services/vehicle.service';
import { TripsService } from '../../core/services/trips.service';
import { TripWrapperModel } from '../../core/models/trips/trip-wrapper.model';
import { DataService } from '../../core/services/service-resolvers/data.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.page.html',
  styleUrls: ['./trips.page.scss'],
})
export class TripsPage implements OnInit, OnDestroy {
  private initialTripsSub$: Subscription;
  private userRegistrationNumbersSub$: Subscription;
  private infiniteScrollSub$: Subscription;
  private tripsListSub$: Subscription;
  private registrationNumbers: Array<RegistrationNumberModel>;
  private page: number;
  
  readonly starSymbol = '*';
  showRegNumbersDropdown: boolean;
  registrationNumbersDropdown: Array<RegistrationNumberModel>;
  isLoading: boolean;
  isIOS: boolean;
  tripWrapper: TripWrapperModel;

  constructor(
    private router: Router,
    private platform: Platform,
    private tripsService: TripsService,
    private actionsheetCntrl: ActionSheetController,
    private translateService: TranslateService,
    private vehicleService: VehicleService,
    private alertCntrl: AlertController,
    private loadingCntrl: LoadingController,
    private tripDataService: DataService) { }

  ngOnInit(): void {
    this.page = 1;
    this.isLoading = true;
    this.isIOS = this.platform.is('ios');

    this.initialTripsSub$ = this.getAllTrips(null)
      .subscribe(_ => this.isLoading = false,
      () => this.isLoading = false,
      () => this.isLoading = false);

    this.userRegistrationNumbersSub$ = this.vehicleService.registrationNumbers
      .subscribe(numbers => {
        this.registrationNumbers = numbers;
        this.registrationNumbersDropdown = [];
        this.registrationNumbersDropdown.push(...this.registrationNumbers);
        this.registrationNumbersDropdown.unshift({
           number: this.starSymbol,
           shortDescription: '',
           vehicleType: ''
        });
      });

    this.tripsListSub$ = this.tripsService.tripWrapper
      .subscribe(tuple => {
        if (!tuple) {
          return;
        }

        const [wrapper, tripModel] = tuple;

        if (this.tripWrapper?.trips) {
          const updated = this.tripWrapper.trips.find(t => t.id === tripModel?.id);
          if (updated) {
            Object.assign(updated, tripModel);
          }

          if (wrapper) {
            // on add new we push it to the top of the array, when fetch existing ones we push them to the end of the array
            const tripsToAdd = wrapper.trips.filter(x => !this.tripWrapper.trips.map(t => t.id).includes(x.id));

            tripsToAdd.forEach(trip => {
              const isTripToAddGreaterThanAnyFromExisting = this.tripWrapper.trips.some(t => trip.id > t.id);

              if (isTripToAddGreaterThanAnyFromExisting) {
                this.tripWrapper.trips.unshift(trip);
              } else {
                this.tripWrapper.trips.push(trip);
              }
            }); 
          }
        }
      });
  }

  ngOnDestroy(): void {
    if (this.initialTripsSub$) {
      this.initialTripsSub$.unsubscribe();
    }

    if (this.tripsListSub$) {
      this.tripsListSub$.unsubscribe();
    }

    if (this.userRegistrationNumbersSub$) {
      this.userRegistrationNumbersSub$.unsubscribe();
    }

    if (this.infiniteScrollSub$) {
      this.infiniteScrollSub$.unsubscribe();
    }
  }

  onNavigateBack(): void {
    this.router.navigate(['']);
  }

  onNavigateToForm(): void {
    this.tripDataService.setData = { registrationNumbers: this.registrationNumbers };
    this.router.navigate(['trips', 'trip-add']);
  }

  async presentActionSheet(id: number): Promise<void> {
    if (!id) {
      return;
    }

    const actionSheet = await this.actionsheetCntrl.create({
      buttons: [{
        text: this.translateService.instant('Edit'),
        icon: 'eye-outline',
        handler: () => {
          this.tripDataService.setData = {
            tripModel: this.tripWrapper.trips.find(t => t.id === id),
            tripId: id,
            registrationNumbers: this.registrationNumbers
          };

          this.router.navigate(['trips', 'trip-edit', id]);
        }
      }, {
        text: this.translateService.instant('Delete'),
        icon: 'trash-outline',
        handler: async () => {
          await this.onDeleteTrip(id);
        }
      }, {
        text: this.translateService.instant('Cancel'),
        icon: 'close',
        handler: () => { }
      }]
    });
    await actionSheet.present();
  }

  loadData($event: any): void {
    if (this.tripWrapper?.totalCount === 0) {
      $event.target.disabled = true;
      return;
    }

    this.infiniteScrollSub$ = this.getAllTrips($event)?.subscribe(_ => _, () => $event.target.complete(), () => $event.target.complete());
  }

  private async onDeleteTrip(id: number): Promise<void> {
    const alert = await this.alertCntrl.create({
      cssClass: 'delete-vehicle-alert',
      header: this.translateService.instant('Trip will be permanently deleted!'),
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

            this.tripsService.delete(id)
              .subscribe(_ => 
                this.tripWrapper.trips = this.tripWrapper.trips.filter(t => t.id !== id),
                () => loading.dismiss(), 
                () => loading.dismiss());
          }
        }
      ]
    });

    await alert.present();
  }

  togggleRegNumbersDropdown(): void {
    this.showRegNumbersDropdown = !this.showRegNumbersDropdown;
  }

  filterTripList($event): void {
    const selectedRegNumber = $event?.detail?.value;

    if (!selectedRegNumber || !this.tripWrapper || !this.tripWrapper.trips) {
      return;
    }

    // TODO FIX
    throw new Error('fix me pls');
    if (selectedRegNumber === this.starSymbol) {
      this.tripWrapper.trips = this.tripWrapper.trips.filter(t => t.registrationNumber !== selectedRegNumber);
    } else {
      this.tripWrapper.trips = this.tripWrapper.trips.filter(t => t.registrationNumber === selectedRegNumber);
    }
  }

  private getAllTrips($event: any): Observable<TripWrapperModel> {
    if (this.tripWrapper && this.tripWrapper.trips && this.tripWrapper.totalCount === this.tripWrapper.trips.length) {
      if ($event) {
        $event.target.disabled = true;
        $event.target.complete();
      }

      return null;
    }

    return this.tripsService.fetchAll(this.page)
      .pipe(map(wrapper => {
        if (this.page === 1) { // initial start
          this.tripWrapper = wrapper;
        }

        if (!this.tripWrapper.trips || this.tripWrapper.totalCount === this.tripWrapper.trips.length) {
          if ($event) {
            $event.target.disabled = true;
          }
        }

        if (this.page > 1 || this.tripWrapper.totalCount === 0) {
          if ($event) {
            $event.target.complete();
          }
        }

        if (!wrapper.trips || wrapper.trips.length === 0) {
          if ($event) {
            $event.target.disabled = true;
          }
        }

        this.page++;
        return wrapper;
      }));
  }
}
