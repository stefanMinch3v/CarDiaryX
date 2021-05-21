import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SettingsService } from './core/services/settings.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  private themeSub: Subscription;

  appPages = [
    {
      title: 'My Vehicles',
      url: '/app/tabs/schedule',
      icon: 'calendar'
    },
    {
      title: 'Taxes/Fees',
      url: '/app/tabs/speakers',
      icon: 'people'
    },
    {
      title: 'Trips',
      url: '/app/tabs/map',
      icon: 'map'
    },
    {
      title: 'Refuels',
      url: '/app/tabs/about',
      icon: 'information-circle'
    },
    {
      title: 'Repairs',
      url: '/app/tabs/about',
      icon: 'information-circle'
    },
    {
      title: 'Download PDF',
      url: '/app/tabs/about',
      icon: 'information-circle'
    }
  ];
  
  isDarkTheme: boolean;

  constructor(private settingsService: SettingsService) { }

  ngOnInit(): void {
    this.themeSub = this.settingsService.currentTheme.subscribe(isDarkTheme => this.isDarkTheme = isDarkTheme);
  }

  ngOnDestroy(): void {
    if (this.themeSub) {
      this.themeSub.unsubscribe();
    }
  }
}
