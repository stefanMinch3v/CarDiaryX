import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { DataService } from './data.service';
 
@Injectable({
  providedIn: 'root'
})
export class DataResolverService implements Resolve<any> {
  constructor(private dataService: DataService) { }
 
  resolve(): any {
    return this.dataService.getData;
  }
}