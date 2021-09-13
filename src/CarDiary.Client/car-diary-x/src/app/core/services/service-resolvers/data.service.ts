import { Injectable } from '@angular/core';
 
@Injectable({
  providedIn: 'root'
})
export class DataService {
  private data: any;
 
  set setData(data: any) {
    this.data = data;
  }
 
  get getData(): any {
    return this.data;
  }
}
