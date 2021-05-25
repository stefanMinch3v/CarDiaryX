import { NgModule } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ProfilePageRoutingModule } from './profile-routing.module';
import { ProfilePage } from './profile.page';
import { CoreModule } from '../../core/modules/core.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    IonicModule,
    ProfilePageRoutingModule,
    ReactiveFormsModule,
    CoreModule
  ],
  declarations: [ProfilePage, ChangePasswordComponent]
})
export class ProfilePageModule {}
