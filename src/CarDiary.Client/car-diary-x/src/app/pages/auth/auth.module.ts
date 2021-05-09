import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { AuthPageRoutingModule } from './auth-routing.module';
import { AuthPage } from './auth.page';

@NgModule({
  imports: [
    IonicModule,
    AuthPageRoutingModule,
    CoreModule
  ],
  declarations: [AuthPage]
})
export class AuthPageModule {}
