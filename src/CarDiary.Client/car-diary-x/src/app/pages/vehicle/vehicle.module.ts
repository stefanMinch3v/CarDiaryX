import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { VehiclePageRoutingModule } from './vehicle-routing.module';
import { VehiclePage } from './vehicle.page';

@NgModule({
  imports: [
    IonicModule,
    VehiclePageRoutingModule,
    CoreModule,
    ReactiveFormsModule
  ],
  declarations: [VehiclePage, VehicleFormComponent]
})
export class VehiclePageModule {}
