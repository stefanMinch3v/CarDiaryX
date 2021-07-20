import { Injectable } from '@angular/core';
import { Plugins } from '@capacitor/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private readonly DEFAULT_LANGUAGE = 'dk';
  private readonly ALLOWED_LANGUAGES = ['en', 'dk'];
  private readonly LANGUAGE_KEY = 'language';

  private readonly THEME_KEY = 'isDarkTheme';
  private readonly VEHICLE_FILTER_SHOW_LIST_KEY = "showVehicleList";

  private readonly CURRENCY_KEY = 'currency';
  private readonly DEFAULT_CURRENCY = 'dkk';
  private readonly ALLOWED_CURRENCIES = ['dkk', 'eur'];

  private _langSub$ = new BehaviorSubject<string>(null);
  private _themeSub$ = new BehaviorSubject<boolean>(false);
  private _currencySub$ = new BehaviorSubject<string>(null);
  private _showVehicleList$ = new BehaviorSubject<boolean>(false);

  constructor(private translate: TranslateService) {
    this.setupSubscriptions();
  }

  get currentLanguage(): Observable<string> {
    return this._langSub$.asObservable()
      .pipe(map(lang => {
        if (!lang) {
          return this.DEFAULT_LANGUAGE;
        }

        return lang;
      }));
  }

  set setCurrentLanguage(lang: 'en' | 'dk') {
    this._langSub$.next(lang);
    Plugins.Storage.set({ key: this.LANGUAGE_KEY, value: lang });
  }

  get currentTheme(): Observable<boolean> {
    return this._themeSub$.asObservable();
  }

  set setCurrentTheme(theme: boolean) {
    this._themeSub$.next(theme);
    Plugins.Storage.set({ key: this.THEME_KEY, value: String(theme) });
  }

  get currentCurrency(): Observable<string> {
    return this._currencySub$.asObservable()
      .pipe(map(currency => {
        if (!currency) {
          return this.DEFAULT_CURRENCY;
        }

        return currency;
      }));
  }

  set setCurrentCurrency(currency: 'dkk' | 'eur') {
    this._currencySub$.next(currency);
    Plugins.Storage.set({ key: this.CURRENCY_KEY, value: currency });
  }

  get currentVehicleFilter(): Observable<boolean> {
    return this._showVehicleList$.asObservable();
  }

  set setCurrentVehicleFilter(showList: boolean) {
    this._showVehicleList$.next(showList);
    Plugins.Storage.set({ key: this.VEHICLE_FILTER_SHOW_LIST_KEY, value: String(showList) });
  }

  private setupSubscriptions(): void {
    this.translate.addLangs(this.ALLOWED_LANGUAGES);

    Plugins.Storage.get({ key: this.LANGUAGE_KEY })
      .then(storedLang => {
        if (!storedLang || !storedLang.value || !this.ALLOWED_LANGUAGES.includes(storedLang.value)) {
          return;
        }

        // possible browser switch
        // const browserLang = translate.getBrowserLang();
        // translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');

        this.translate.setDefaultLang(storedLang.value);
        this._langSub$.next(storedLang.value);
      });

    Plugins.Storage.get({ key: this.THEME_KEY })
      .then(storedTheme => {
        if (!storedTheme) {
          return;
        }

        this._themeSub$.next(JSON.parse(storedTheme.value));
      });

    Plugins.Storage.get({ key: this.CURRENCY_KEY })
      .then(storedCurrency => {
        if (!storedCurrency || !storedCurrency.value || !this.ALLOWED_CURRENCIES.includes(storedCurrency.value)) {
          return;
        }

        this._currencySub$.next(storedCurrency.value);
      });

    Plugins.Storage.get({ key: this.VEHICLE_FILTER_SHOW_LIST_KEY })
      .then(storedFilter => {
        if (!storedFilter) {
          return;
        }

        this._showVehicleList$.next(JSON.parse(storedFilter.value));
      });
  }
}
