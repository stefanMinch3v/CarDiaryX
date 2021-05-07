import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private static readonly TOKEN_KEY = 'token';
  private static readonly EXPIRE_KEY = 'expiration';

  public isUserAuthenticated() {
    const expirationTime = new Date(parseInt(this.getExpirationTime()));
    if (window.localStorage.getItem(AuthService.TOKEN_KEY) !== null && expirationTime > new Date) {
      return true;
    }

    return false;
  }

  public authenticateUser(token, expiration) {
    window.localStorage.setItem(AuthService.TOKEN_KEY, token);
    window.localStorage.setItem(AuthService.EXPIRE_KEY, expiration);
  }

  public deauthenticateUser() {
    this.removeToken();
    this.removeExpirationTime();
  }

  public getToken() {
    return window.localStorage.getItem(AuthService.TOKEN_KEY);
  }

  private getExpirationTime() {
    return window.localStorage.getItem(AuthService.EXPIRE_KEY);
  }

  private removeExpirationTime() {
    window.localStorage.removeItem(AuthService.EXPIRE_KEY);
  }

  private removeToken() {
    window.localStorage.removeItem(AuthService.TOKEN_KEY);
  }
}