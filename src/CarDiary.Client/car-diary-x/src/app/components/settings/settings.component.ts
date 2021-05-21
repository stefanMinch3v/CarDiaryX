import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../core/services/settings.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy {
  private langSub: Subscription;
  private themeSub: Subscription;
  private currencySub: Subscription;
  private langFlags = ['denmark-flag', 'united-kingdom-flag'];
  private currencyFlags = ['denmark-flag', 'european-union-flag'];
  currentLanguage: string;
  currentCurrency: string;
  isDarkTheme: boolean;

  constructor(
    private modalCntrl: ModalController, 
    private settingsService: SettingsService) {}

  ngOnInit(): void {
    this.langSub = this.settingsService.currentLanguage.subscribe(lang => this.currentLanguage = lang);
    this.themeSub = this.settingsService.currentTheme.subscribe(isDarkTheme => this.isDarkTheme = isDarkTheme);
    this.currencySub = this.settingsService.currentCurrency.subscribe(currency => this.currentCurrency = currency);
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
    }

    if (this.themeSub) {
      this.themeSub.unsubscribe();
    }

    if (this.currentCurrency) {
      this.currencySub.unsubscribe();
    }
  }

  onDismissModal(): void {
    this.modalCntrl.dismiss({ 'dismissed': true });
  }

  onChangeLanguage(event: any): void {
    const selectedRawLang = event?.detail?.value;

    if (!selectedRawLang) {
      return;
    }

    const selectedLang = selectedRawLang !== 'en' && selectedRawLang !== 'dk' ? 'en' : selectedRawLang;
    this.settingsService.setCurrentLanguage = selectedLang;
  }

  onThemeChange(event: any): void {
    this.isDarkTheme = event;
    this.settingsService.setCurrentTheme = this.isDarkTheme;
  }

  onCurrencyChange(event: any): void {
    const selectedRawCurrency = event?.detail?.value;

    if (!selectedRawCurrency) {
      return;
    }

    const selectedCurrency = selectedRawCurrency !== 'dkk' && selectedRawCurrency !== 'eur' ? 'dkk' : selectedRawCurrency;
    this.settingsService.setCurrentCurrency = selectedCurrency;
  }
  
  setLanguageIconFlags(): void {
    setTimeout(() => {
      // According to the class style "div.alert-radio-group button" to get html elements
      let buttonElements = document.querySelectorAll('div.alert-radio-group button');
 
      // Determine whether the obtained element is not null
      if (!buttonElements.length || buttonElements.length !== this.langFlags.length) {   
        return;
      } else {
        // If it is not empty, loop through the obtained html element (that is, the html element where the information of the AlertController list is traversed)
        for (let index = 0; index < buttonElements.length; index++) {
          // According to the subscript to take the html element
          let buttonElement = buttonElements[index];
 
          // Then take the information in the list according to the html element
          let optionLabelElement = buttonElement.querySelector('.alert-radio-label');
          let flag = this.langFlags[index];

          // Splice the picture name to display the picture, pay attention to the picture naming, must be consistent with the binding field, then add Image for this element   
          optionLabelElement.innerHTML += '<ion-img src="assets/icon/' + flag + '.svg" style="width:20px;height:20px;float:right;margin-right: 15px;"></ion-image>';
        }
      }
    }, 60);
  }

  setCurrencyIconFlags(): void {
    setTimeout(() => {
      // According to the class style "div.alert-radio-group button" to get html elements
      let buttonElements = document.querySelectorAll('div.alert-radio-group button');
 
      // Determine whether the obtained element is not null
      if (!buttonElements.length || buttonElements.length !== this.langFlags.length) {   
        return;
      } else {
        // If it is not empty, loop through the obtained html element (that is, the html element where the information of the AlertController list is traversed)
        for (let index = 0; index < buttonElements.length; index++) {
          // According to the subscript to take the html element
          let buttonElement = buttonElements[index];
 
          // Then take the information in the list according to the html element
          let optionLabelElement = buttonElement.querySelector('.alert-radio-label');
          let flag = this.currencyFlags[index];

          // Splice the picture name to display the picture, pay attention to the picture naming, must be consistent with the binding field, then add Image for this element   
          optionLabelElement.innerHTML += '<ion-img src="assets/icon/' + flag + '.svg" style="width:20px;height:20px;float:right;margin-right: 15px;"></ion-image>';
        }
      }
    }, 60);
  }
}
