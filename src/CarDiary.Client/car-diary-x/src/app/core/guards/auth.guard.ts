import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | 
      UrlTree> | 
      Promise<boolean | 
      UrlTree> | 
      boolean | 
      UrlTree {
    return this.check(state);
  }
  
  check(state: RouterStateSnapshot): boolean {
    if (this.authService.isUserAuthenticated()) {
        return true;
    }

    this.authService.deauthenticateUser();
    this.router.navigate([''], { queryParams: { returnUrl: state.url }});
    return false;
}
}
