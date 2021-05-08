import { Route } from '@angular/compiler/src/core';
import { Injectable } from '@angular/core';
import { Router, CanLoad, UrlSegment } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanLoad {
  constructor(private authService: AuthService, private router: Router) {}

  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.isUserAuthenticated()
      .then(isAuth => {
        if (isAuth) {
          return true;
        }

        this.authService.deauthenticateUser();
        this.router.navigate(['auth']);
        return false;
      });
  }
}
