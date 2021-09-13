import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataResolverService } from '../../core/services/service-resolvers/data-resolver.service';
import { TripAddComponent } from './trip-add/trip-add.component';
import { TripEditComponent } from './trip-edit/trip-edit.component';

import { TripsPage } from './trips.page';

const routes: Routes = [
  {
    path: '',
    component: TripsPage,
    resolve: {
      extraData: DataResolverService
    }
  },
  {
    path: 'trip-add',
    component: TripAddComponent,
    resolve: {
      extraData: DataResolverService
    }
  },
  {
    path: 'trip-edit/:id',
    component: TripEditComponent,
    resolve: {
      extraData: DataResolverService
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TripsPageRoutingModule {}
