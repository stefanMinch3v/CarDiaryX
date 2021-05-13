import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoadingController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { loadingOverlays } from '../../core/constants/loadingOverlays';
import { I18nService } from '../../core/services/i18n.service';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss'],
})
export class TabsPage implements OnInit, OnDestroy {
  private langSub: Subscription;

  constructor(
    private i18nService: I18nService, 
    private translateService: TranslateService,
    private loadingCntrl: LoadingController) { }
  
  ngOnInit(): void {
    this.langSub = this.i18nService.currentLanguage.subscribe(lang => this.translateService.use(lang));
    this.loadingCntrl.getTop().then(overLay => overLay?.id === loadingOverlays.initialRegistration ? overLay.dismiss() : null);
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }
}
