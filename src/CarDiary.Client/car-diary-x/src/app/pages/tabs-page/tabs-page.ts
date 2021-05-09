import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { I18nService } from '../../core/services/i18n.service';

@Component({
  selector: 'app-tabs-page',
  templateUrl: './tabs-page.html',
  styleUrls: ['./tabs-page.scss'],
})
export class TabsPage implements OnInit, OnDestroy {
  private langSub: Subscription;

  constructor(private i18nService: I18nService, private translateService: TranslateService) { }
  
  ngOnInit(): void {
    this.langSub = this.i18nService.currentLanguage.subscribe(lang => this.translateService.use(lang));
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }
  }
}
