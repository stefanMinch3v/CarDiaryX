import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnonymousGuard } from '../../core/guards/anonymous.guard';

import { AuthPage } from './auth.page';

const routes: Routes = [
  {
    path: '',
    component: AuthPage,
    canActivate: [AnonymousGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthPageRoutingModule {}
