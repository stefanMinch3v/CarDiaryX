import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { HistoryPageRoutingModule } from './history-routing.module';
import { HistoryPage } from './history.page';
import { CoreModule } from 'src/app/core/modules/core.module';

@NgModule({
  imports: [
    IonicModule,
    HistoryPageRoutingModule,
    CoreModule
  ],
  declarations: [HistoryPage]
})
export class HistoryPageModule {}
