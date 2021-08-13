import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ChangePasswordModel } from '../models/identity/change-password.model';
import { LoginResponseModel } from '../models/identity/login-response.model';
import { LoginModel } from '../models/identity/login.model';
import { RegisterModel } from '../models/identity/register.model';
import { UpdateUserModel } from '../models/identity/update-user.model';
import { UserDetailsModel } from '../models/identity/user-details.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  private readonly IDENTITY_V1 = '/v1/identity';

  constructor(private http: HttpClient, private authService: AuthService) { }

  login(login: LoginModel): Observable<LoginResponseModel> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/login`;
    return this.http.post<LoginResponseModel>(url, login);
  }

  register(register: RegisterModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/register`;
    return this.http.post(url, register);
  }

  logout(): void {
    this.authService.deauthenticateUser();
  }

  changePassword(changePassword: ChangePasswordModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/password`;
    return this.http.put(url, changePassword);
  }

  deleteAccount(confirmPassword: string): Observable<any> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/delete`;
    return this.http.request('delete', url, { body: { confirmPassword: confirmPassword } });
  }

  get(): Observable<UserDetailsModel> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/details`;
    return this.http.get<UserDetailsModel>(url);
  }

  update(updateUser: UpdateUserModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.IDENTITY_V1}/update`;
    return this.http.put(url, updateUser);
  }
}
