import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TripModel } from '../models/trips/trip.model';

@Injectable()
export class TripsService {
  private readonly TRIPS_V1 = '/v1/trips';
  
  constructor(private http: HttpClient) { }

  add(tripModel: TripModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.TRIPS_V1}/add`;
    return this.http.post(url, tripModel);
  }
}
