import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { TabsPagePageRoutingModule } from './tabs-page-routing.module';
import { TabsPage } from './tabs-page';

@NgModule({
  imports: [
    IonicModule,
    TabsPagePageRoutingModule,
    CoreModule
  ],
  declarations: [TabsPage]
})
export class TabsPageModule {}
