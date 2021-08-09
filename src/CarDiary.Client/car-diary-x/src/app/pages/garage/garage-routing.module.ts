import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
import { AddVehicleComponent } from './add-vehicle/add-vehicle.component';

import { GaragePage } from './garage.page';

const routes: Routes = [
  {
    path: '',
    component: GaragePage
  },
  {
    path: 'add-vehicle',
    component: AddVehicleComponent
  },
  {
    path: 'vehicle-details/:registrationNumber',
    component: VehicleDetailsComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GaragePageRoutingModule {}
