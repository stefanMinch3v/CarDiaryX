import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { RegistrationNumberModel } from '../models/vehicles/registration-number.model';
import { VehicleSharedModel } from '../models/vehicles/vehicle-shared.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly VEHICLES_V1 = '/v1/vehicles';
  private _regNumbersSub$ = new BehaviorSubject<Array<RegistrationNumberModel>>([]);
  
  constructor(private http: HttpClient) { }

  addToUser(registrationNumber: string): Observable<any> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/add-to-user`;
    return this.http.post(url, { registrationNumber })
      .pipe(switchMap(_ => this.fetchAllRegistrationNumbers()));
  }

  getInformation(registrationNumber: string): Observable<VehicleSharedModel> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/get-information`;
    return this.http.get<VehicleSharedModel>(url, { params: { registrationNumber } });
  }

  removeFromUser(registrationNumber: string): Observable<any> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/remove-from-user`;
    return this.http.delete(url, { params: { registrationNumber } })
      .pipe(switchMap(_ => this.fetchAllRegistrationNumbers()));
  }

  get registrationNumbers(): Observable<Array<RegistrationNumberModel>> {
    return this._regNumbersSub$.asObservable();
  }

  fetchAllRegistrationNumbers(): Observable<Array<RegistrationNumberModel>> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/get-all-registration-numbers`;
    return this.http.get<Array<RegistrationNumberModel>>(url)
      .pipe(map(regNumbers => {
        this._regNumbersSub$.next(regNumbers);
        return regNumbers;
      }));
  }
}
