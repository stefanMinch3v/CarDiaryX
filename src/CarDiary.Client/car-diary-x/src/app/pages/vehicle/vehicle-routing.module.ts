import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';

import { VehiclePage } from './vehicle.page';

const routes: Routes = [
  {
    path: '',
    component: VehiclePage
  },
  {
    path: 'vehicle-form',
    component: VehicleFormComponent
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
export class VehiclePageRoutingModule {}
