import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { TripOutputModel } from '../models/trips/trip-output.model';
import { TripWrapperModel } from '../models/trips/trip-wrapper.model';
import { TripInputModel } from '../models/trips/trip-input.model';

@Injectable()
export class TripsService {
  private readonly TRIPS_V1 = '/v1/trips';
  private _tripsSub$ = new BehaviorSubject<[tripWrapperModel: TripWrapperModel, updatedTripModel: TripOutputModel]>(null);

  constructor(private http: HttpClient) { }

  get tripWrapper(): Observable<[TripWrapperModel, TripOutputModel]> {
    return this._tripsSub$.asObservable();
  }

  add(tripModel: TripInputModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.TRIPS_V1}/add`;
    return this.http.post(url, { trip: tripModel })
      .pipe(take(1), switchMap(_ => this.fetchAll()));
  }

  fetchAll(page: number = 1): Observable<TripWrapperModel> {
    const url = `${environment.host.baseUrl}${this.TRIPS_V1}/get-all?page=${page}`;
    return this.http.get<TripWrapperModel>(url)
      .pipe(map(w => {
        this._tripsSub$.next([w, null]);
        return w;
      }));
  }

  update(id: number, tripModel: TripInputModel): Observable<any> {
    const url = `${environment.host.baseUrl}${this.TRIPS_V1}/update`;
    return this.http.put(url, { id: id, trip: tripModel })
      .pipe(take(1), map(_ => {
        this._tripsSub$.next([null, this.mapTo(id, tripModel)]);
      }));
  }

  delete(id: number): Observable<any> {
    const url = `${environment.host.baseUrl}${this.TRIPS_V1}/delete/${id}`;
    return this.http.delete(url)
      .pipe(take(1));
  }

  private mapTo(id: number, tripModel: TripInputModel): TripOutputModel {
    if (!tripModel) {
      return null;
    }

    return {
      id: id,
      cost: tripModel.cost,
      note: tripModel.note,
      distance: tripModel.distance,
      arrivalAddress: tripModel.arrivalAddress,
      departureAddress: tripModel.departureAddress,
      registrationNumber: tripModel.registrationNumber,
      arrivalDate: new Date(tripModel.arrivalDate).toISOString(),
      departureDate: new Date(tripModel.departureDate).toISOString()
    };
  }
}
