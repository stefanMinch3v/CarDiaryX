import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginResponseModel } from '../models/identity/login-response.model';
import { LoginModel } from '../models/identity/login.model';
import { RegisterModel } from '../models/identity/register.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  constructor(private http: HttpClient, private authService: AuthService) { }

  public login(login: LoginModel): Observable<LoginResponseModel> {
    const url = environment.host.baseUrl + '/v1/identity/login';
    return this.http.post<LoginResponseModel>(url, login);
  }

  public register(register: RegisterModel) {
    const url = environment.host.baseUrl + '/v1/identity/register';
    return this.http.post(url, register);
  }

  public logout(): void {
    this.authService.deauthenticateUser();
  }
}
