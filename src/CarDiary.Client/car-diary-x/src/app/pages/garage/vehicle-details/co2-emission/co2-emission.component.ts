import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IonSlides, ModalController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../../../core/services/settings.service';

@Component({
  selector: 'app-co2-emission',
  templateUrl: './co2-emission.component.html',
  styleUrls: ['./co2-emission.component.scss']
})
export class Co2EmissionComponent implements OnInit, OnDestroy {
  private themeSub$: Subscription;
  @ViewChild(IonSlides) slides: IonSlides;
  isIOS: boolean;
  isDarkMode: boolean;
  sliderOpts = {
    zoom: true
  };
  
  constructor(private modalCntrl: ModalController, private settingsService: SettingsService) { }

  ngOnInit(): void {
    this.themeSub$ = this.settingsService.currentTheme.subscribe(isDarkMode => this.isDarkMode = isDarkMode);
  }

  ngOnDestroy(): void {
    if (this.themeSub$) {
      this.themeSub$.unsubscribe();
    }
  }

  ionViewWillEnter(): void {
    this.slides.update();
  }

  onDismissModal(): void {
    this.modalCntrl.dismiss({ 'dismissed': true });
  }

  async zoom(zoomIn: boolean): Promise<void> {
    const slider = await this.slides.getSwiper();
    const zoom = slider.zoom;
    zoomIn ? zoom.in() : zoom.out();
  }
}