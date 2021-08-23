import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { TabsPagePageRoutingModule } from './tabs-page-routing.module';
import { TabsPage } from './tabs-page';
import { AddTripComponent } from './add-trip/add-trip.component';
import { DawaAddressService } from '../../core/services/dawa-address.service';
import { IonicSelectableModule } from 'ionic-selectable';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    IonicModule,
    TabsPagePageRoutingModule,
    CoreModule,
    ReactiveFormsModule,
    IonicSelectableModule
  ],
  declarations: [TabsPage, AddTripComponent],
  providers: [DawaAddressService]
})
export class TabsPageModule {}
