import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { DatePipe } from '@angular/common';
import { Co2EmissionComponent } from '../../components/co2-emission/co2-emission.component';
import { CoreModule } from '../../core/modules/core.module';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
import { AddVehicleComponent } from './add-vehicle/add-vehicle.component';
import { GaragePageRoutingModule } from './garage-routing.module';
import { GaragePage } from './garage.page';

@NgModule({
  imports: [
    IonicModule,
    GaragePageRoutingModule,
    CoreModule,
    ReactiveFormsModule
  ],
  declarations: [GaragePage, AddVehicleComponent, VehicleDetailsComponent, Co2EmissionComponent],
  providers: [DatePipe]
})
export class GaragePageModule {}
