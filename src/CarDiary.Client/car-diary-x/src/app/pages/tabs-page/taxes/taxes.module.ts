import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../../core/modules/core.module';
import { TaxesPageRoutingModule } from './taxes-routing.module';
import { TaxesPage } from './taxes.page';

@NgModule({
  imports: [
    IonicModule,
    TaxesPageRoutingModule,
    CoreModule
  ],
  declarations: [TaxesPage]
})
export class TaxesPageModule {}
