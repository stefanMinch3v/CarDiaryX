import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { SettingsComponent } from '../../components/settings/settings.component';
import { I18nService } from '../../core/services/i18n.service';

@Component({
  selector: 'app-fuel',
  templateUrl: './fuel.page.html',
  styleUrls: ['./fuel.page.scss'],
})
export class FuelPage implements OnInit, OnDestroy {
  private langSub: Subscription;

  constructor(
    private i18nService: I18nService, 
    private translateService: TranslateService,
    private modalCntrl: ModalController) { }

  ngOnInit(): void {
    this.langSub = this.i18nService.currentLanguage.subscribe(lang => this.translateService.use(lang));
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
