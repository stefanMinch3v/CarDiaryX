import { Injectable } from '@angular/core';
import { Plugins } from '@capacitor/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private static readonly DEFAULT_LANGUAGE = 'dk';
  private static readonly ALLOWED_LANGUAGES = ['en', 'dk'];
  private static readonly LANGUAGE_KEY = 'language';

  private static readonly THEME_KEY = 'isDarkTheme';

  private static readonly CURRENCY_KEY = 'currency';
  private static readonly DEFAULT_CURRENCY = 'dkk';
  private static readonly ALLOWED_CURRENCIES = ['dkk', 'eur'];

  private _langSub = new BehaviorSubject<string>(null);
  private _themeSub = new BehaviorSubject<boolean>(false);
  private _currencySub = new BehaviorSubject<string>(null);

  constructor(private translate: TranslateService) {
    this.setupSubscriptions();
  }

  get currentLanguage(): Observable<string> {
    return this._langSub.asObservable()
      .pipe(map(lang => {
        if (!lang) {
          return SettingsService.DEFAULT_LANGUAGE;
        }

        return lang;
      }));
  }

  set setCurrentLanguage(lang: 'en' | 'dk') {
    this._langSub.next(lang);
    Plugins.Storage.set({ key: SettingsService.LANGUAGE_KEY, value: lang });
  }

  get currentTheme(): Observable<boolean> {
    return this._themeSub.asObservable();
  }

  set setCurrentTheme(theme: boolean) {
    this._themeSub.next(theme);
    Plugins.Storage.set({ key: SettingsService.THEME_KEY, value: String(theme) });
  }

  get currentCurrency(): Observable<string> {
    return this._currencySub.asObservable()
      .pipe(map(currency => {
        if (!currency) {
          return SettingsService.DEFAULT_CURRENCY;
        }

        return currency;
      }));
  }

  set setCurrentCurrency(currency: 'dkk' | 'eur') {
    this._currencySub.next(currency);
    Plugins.Storage.set({ key: SettingsService.CURRENCY_KEY, value: currency });
  }

  private setupSubscriptions(): void {
    this.translate.addLangs(SettingsService.ALLOWED_LANGUAGES);

    Plugins.Storage.get({ key: SettingsService.LANGUAGE_KEY })
      .then(storedLang => {
        if (!storedLang || !storedLang.value || !SettingsService.ALLOWED_LANGUAGES.includes(storedLang.value)) {
          return;
        }

        // possible browser switch
        // const browserLang = translate.getBrowserLang();
        // translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');

        this.translate.setDefaultLang(storedLang.value);
        this._langSub.next(storedLang.value);
      });

    Plugins.Storage.get({ key: SettingsService.THEME_KEY })
      .then(storedTheme => {
        if (!storedTheme) {
          return;
        }

        this._themeSub.next(JSON.parse(storedTheme.value));
      });

    Plugins.Storage.get({ key: SettingsService.CURRENCY_KEY })
      .then(storedCurrency => {
        if (!storedCurrency || !storedCurrency.value || !SettingsService.ALLOWED_CURRENCIES.includes(storedCurrency.value)) {
          return;
        }

        this._currencySub.next(storedCurrency.value);
      });
  }
}
