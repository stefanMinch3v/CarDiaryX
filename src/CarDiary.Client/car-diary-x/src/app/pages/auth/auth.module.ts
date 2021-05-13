import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { CoreModule } from '../../core/modules/core.module';
import { AuthPageRoutingModule } from './auth-routing.module';
import { AuthPage } from './auth.page';
import { RegisterComponent } from './register/register.component';

@NgModule({
  imports: [
    IonicModule,
    AuthPageRoutingModule,
    ReactiveFormsModule,
    CoreModule
  ],
  declarations: [AuthPage, RegisterComponent]
})
export class AuthPageModule {}
