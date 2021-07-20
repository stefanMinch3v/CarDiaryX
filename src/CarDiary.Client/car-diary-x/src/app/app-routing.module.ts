import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AnonymousGuard } from './core/guards/anonymous.guard';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'tabs',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./pages/auth/auth.module').then( m => m.AuthPageModule),
    canActivate: [AnonymousGuard]
  },
  {
    path: 'tabs',
    loadChildren: () => import('./pages/tabs-page/tabs-page.module').then( m => m.TabsPageModule),
    canLoad: [AuthGuard]
  },
  {
    path: 'profile',
    loadChildren: () => import('./pages/profile/profile.module').then( m => m.ProfilePageModule),
    canLoad: [AuthGuard]
  },
  {
    path: 'vehicles',
    loadChildren: () => import('./pages/vehicle/vehicle.module').then( m => m.VehiclePageModule),
    canLoad: [AuthGuard]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
