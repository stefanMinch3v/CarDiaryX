import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { FuelPageRoutingModule } from './fuel-routing.module';
import { FuelPage } from './fuel.page';
import { CoreModule } from '../../../core/modules/core.module';

@NgModule({
  imports: [
    IonicModule,
    FuelPageRoutingModule,
    CoreModule
  ],
  declarations: [FuelPage]
})
export class FuelPageModule {}
