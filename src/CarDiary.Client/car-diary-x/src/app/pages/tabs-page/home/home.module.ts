import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { HomePageRoutingModule } from './home-routing.module';
import { CoreModule } from '../../../core/modules/core.module';
import { HomePage } from './home.page';

@NgModule({
  imports: [
    IonicModule,
    HomePageRoutingModule,
    CoreModule],
  exports: [],
  declarations: [HomePage]
})
export class HomePageModule {}