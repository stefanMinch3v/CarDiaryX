import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { TabsPagePageRoutingModule } from './tabs-page-routing.module';
import { TabsPage } from './tabs-page';
import { AddTripComponent } from './add-trip/add-trip.component';
import { DawaAddressService } from '../../core/services/dawa-address.service';
import { IonicSelectableModule } from 'ionic-selectable';
import { ReactiveFormsModule } from '@angular/forms';
import { TripsService } from '../../core/services/trips.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from '../../core/interceptors/error.interceptor';
import { TokenInterceptor } from '../../core/interceptors/token.interceptor';

@NgModule({
  imports: [
    IonicModule,
    TabsPagePageRoutingModule,
    CoreModule,
    ReactiveFormsModule,
    IonicSelectableModule
  ],
  declarations: [TabsPage, AddTripComponent],
  providers: [
    DawaAddressService, 
    TripsService, 
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
export class TabsPageModule {}
