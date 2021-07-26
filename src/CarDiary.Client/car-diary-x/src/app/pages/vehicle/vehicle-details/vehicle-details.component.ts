import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Platform } from '@ionic/angular';
import { VehicleService } from '../../../core/services/vehicle.service';

@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  styleUrls: ['./vehicle-details.component.scss'],
})
export class VehicleDetailsComponent implements OnInit {
  isIOS: boolean;
  registrationNumber: string;
  jsonData: any;

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private platform: Platform,
    private vehicleService: VehicleService) { }

  ngOnInit() {
    this.isIOS = this.platform.is('ios');
    this.route.params.subscribe(queryParams => {
      this.registrationNumber = queryParams.registrationNumber;
      setTimeout(() => 
        this.vehicleService.get(this.registrationNumber)
          .subscribe(result => {
            const json = this.jsonData = JSON.parse(result.jsonData);
            this.jsonData = json?.data;
            console.log(this.jsonData?.data);
          }), 500);
    });
  }

  onNavigateBack(): void {
    return this.location.back();
  }
}
