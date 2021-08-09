import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RegistrationNumberModel } from '../models/vehicles/registration-number.model';
import { VehicleInfoModel } from '../models/vehicles/vehicle-info.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly VEHICLES_V1 = '/v1/vehicles';
  
  constructor(private http: HttpClient) { }

  addToUser(registrationNumber: string): Observable<any> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/add-to-user`;
    return this.http.post(url, { registrationNumber });
  }

  getAllRegistrationNumbers(): Observable<Array<RegistrationNumberModel>> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/get-all-registration-numbers`;
    return this.http.get<Array<RegistrationNumberModel>>(url);
  }

  getInformation(registrationNumber: string): Observable<VehicleInfoModel> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/get-information`;
    return this.http.get<VehicleInfoModel>(url, { params: { registrationNumber } });
  }

  removeFromUser(registrationNumber: string): Observable<any> {
    const url = `${environment.host.baseUrl}${this.VEHICLES_V1}/remove-from-user`;
    return this.http.delete(url, { params: { registrationNumber } });
  }
}
