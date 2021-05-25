import { Injectable } from '@angular/core';
import { Plugins } from '@capacitor/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'token';
  private readonly EXPIRE_KEY = 'expiration';

  public async isUserAuthenticated() {
    const expirationTimeResult = await this.getExpirationTime();
    const tokenResult = await this.getToken();
    if (tokenResult.value !== null && new Date((expirationTimeResult.value)) > new Date) {
      return true;
    }

    return false;
  }

  public authenticateUser(token, expiration): void {
    Plugins.Storage.set({ key: this.TOKEN_KEY, value: token });
    Plugins.Storage.set({ key: this.EXPIRE_KEY, value: expiration });
  }

  public deauthenticateUser(): void {
    this.removeToken();
    this.removeExpirationTime();
  }

  public getToken(): Promise<{ value: string }> {
    return Plugins.Storage.get({ key: this.TOKEN_KEY });
  }

  private getExpirationTime(): Promise<{ value: string }> {
    return Plugins.Storage.get({ key: this.EXPIRE_KEY });
  }

  private removeExpirationTime(): void {
    Plugins.Storage.remove({ key: this.EXPIRE_KEY });
  }

  private removeToken(): void {
    Plugins.Storage.remove({ key: this.TOKEN_KEY });
  }
}