import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { TripsPageRoutingModule } from './trips-routing.module';
import { TripsPage } from './trips.page';
import { CoreModule } from '../../core/modules/core.module';
import { TripsService } from '../../core/services/trips.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from '../../core/interceptors/error.interceptor';
import { TokenInterceptor } from '../../core/interceptors/token.interceptor';
import { IonicSelectableModule } from 'ionic-selectable';
import { TripFormComponent } from './trip-form/trip-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DawaAddressService } from '../../core/services/dawa-address.service';
import { TripAddComponent } from './trip-add/trip-add.component';
import { TripEditComponent } from './trip-edit/trip-edit.component';

@NgModule({
  imports: [
    CoreModule,
    IonicModule,
    TripsPageRoutingModule,
    IonicSelectableModule,
    ReactiveFormsModule
  ],
  declarations: [
    TripsPage,
    TripFormComponent,
    TripAddComponent,
    TripEditComponent
  ],
  providers: [
    TripsService,
    DawaAddressService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})
export class TripsPageModule {}
