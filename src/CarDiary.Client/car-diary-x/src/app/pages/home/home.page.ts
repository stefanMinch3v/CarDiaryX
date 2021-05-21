import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SettingsComponent } from '../../components/settings/settings.component';
import { SettingsService } from '../../core/services/settings.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit, OnDestroy {
  private langSub: Subscription;

  constructor(
    private settingsService: SettingsService, 
    private translateService: TranslateService,
    private modalCntrl: ModalController) { }
  
  ngOnInit(): void {
    this.langSub = this.settingsService.currentLanguage.subscribe(lang => this.translateService.use(lang));
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }

  async presentSettingsModal(): Promise<void> {
    const modal = await this.modalCntrl.create({
      component: SettingsComponent
    });

    return await modal.present();
  }
}
