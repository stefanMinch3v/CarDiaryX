import { Injectable } from '@angular/core';
import { Plugins } from '@capacitor/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class I18nService {
  private static readonly DEFAULT_LANGUAGE = 'dk';
  private static readonly ALLOWED_LANGUAGES = ['en', 'dk'];
  private static readonly LANGUAGE_KEY = 'language';
  private _langSub = new BehaviorSubject<string>(null);

  constructor(private translate: TranslateService) {
    this.translate.addLangs(I18nService.ALLOWED_LANGUAGES);

    Plugins.Storage.get({ key: I18nService.LANGUAGE_KEY })
      .then(storedLang => {
        if (!storedLang || !storedLang.value || !I18nService.ALLOWED_LANGUAGES.includes(storedLang.value)) {
          return;
        }

        // possible browser switch
        // const browserLang = translate.getBrowserLang();
        // translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');

        this.translate.setDefaultLang(storedLang.value);
        this._langSub.next(storedLang.value);
      });
  }

  get currentLanguage(): Observable<string> {
    return this._langSub.asObservable()
      .pipe(map(lang => {
        if (!lang) {
          return I18nService.DEFAULT_LANGUAGE;
        }

        return lang;
      }));
  }

  set setCurrentLanguage(lang: 'en' | 'dk') {
    this._langSub.next(lang);
    Plugins.Storage.set({ key: I18nService.LANGUAGE_KEY, value: lang });
  }
}
