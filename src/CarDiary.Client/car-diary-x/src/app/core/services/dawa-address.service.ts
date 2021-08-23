import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class DawaAddressService {
  constructor(private http: HttpClient) {}

  get(input: string): Observable<any> {
    if (!input) {
      return;
    }

    const url = `https://api.dataforsyningen.dk/autocomplete?q=${input}&type=adresse&caretpos=12&supplerendebynavn=true&stormodtagerpostnumre=true&multilinje=true&fuzzy=`;
    return this.http.get<any>(url)
      .pipe();
  }
}
