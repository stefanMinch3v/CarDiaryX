import { DatePipe, Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalController, Platform } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Co2EmissionComponent } from '../../../components/co2-emission/co2-emission.component';
import { VehicleService } from '../../../core/services/vehicle.service';

@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  styleUrls: ['./vehicle-details.component.scss'],
})
export class VehicleDetailsComponent implements OnInit {
  private vehicleDetailsSub$: Subscription;
  protected readonly DASH_SYMBOL = '-';
  isIOS: boolean;
  registrationNumber: string;
  jsonData: any;
  isLoading: boolean;

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private platform: Platform,
    private vehicleService: VehicleService,
    private modalCntrl: ModalController,
    private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.isIOS = this.platform.is('ios');
    this.vehicleDetailsSub$ = this.route.params
      .pipe(
        switchMap(queryParams => {
          this.registrationNumber = queryParams.registrationNumber;
          return this.vehicleService.getInformation(this.registrationNumber);
        })
      )
      .subscribe(result => {
        this.isLoading = false;
        this.jsonData = JSON.parse(result.jsonData)?.data;
      }, () => this.isLoading = false, () => this.isLoading = false);
  }

  ngOnDestroy(): void {
    if (this.vehicleDetailsSub$) {
      this.vehicleDetailsSub$.unsubscribe();
    }
  }

  ionViewWillLeave(): void {
    if (this.vehicleDetailsSub$) {
      this.vehicleDetailsSub$.unsubscribe();
    }
  }

  onNavigateBack(): void {
    return this.location.back();
  }

  getPropertyOrNotAvailable(propName: any, isDate: boolean = false): any {
    if (!propName) {
      return this.DASH_SYMBOL;
    }

    if (isDate) {
      const value = propName.split('-')[0];

      if (value.length > 2) {
        return this.datePipe.transform(propName, 'dd-MM-yyyy');
      }
    }

    return propName;
  }

  calculateHowOldIsTheVehicle(vehicleAge: any): number {
    return new Date().getFullYear() - vehicleAge;
  }

  getEquipment(equipments: Array<any>): string {
    const result = [];
    for (const equipment of equipments) {
      result.push(equipment?.name);
    }

    return result.join(', ');
  }

  getVehicleYearFromMetaDescription(metaDescription: string): string {
    const splitArr = metaDescription.split(/[.fra]+/);
    const yearString =  splitArr[1]?.trim();

    if (Number.isNaN(yearString) || !yearString) {
      return this.DASH_SYMBOL;
    } else {
      return yearString;
    }
  }

  async presentCO2EmissionModal(jsonProp: any): Promise<void> {
    if (!jsonProp) {
      return;
    }

    const modal = await this.modalCntrl.create({ 
      component: Co2EmissionComponent,
      cssClass: 'transparent-modal'
    });
    await modal.present();
  }
}
