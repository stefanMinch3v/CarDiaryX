import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { I18nService } from '../../core/services/i18n.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit, OnDestroy {
  private langSub: Subscription;
  private flags = ['united-kingdom-flag', 'denmark-flag'];
  currentLanguage: string;

  constructor(
    private modalCntrl: ModalController, 
    private i18nService: I18nService) {}

  ngOnInit(): void {
    this.langSub = this.i18nService.currentLanguage.subscribe(lang => this.currentLanguage = lang);
  }

  ngOnDestroy(): void {
    if (this.langSub) {
      this.langSub.unsubscribe();
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
    this.i18nService.setCurrentLanguage = selectedLang;
  }
  
  setIconFlags(): void {
    setTimeout(() => {
      // According to the class style "div.alert-radio-group button" to get html elements
      let buttonElements = document.querySelectorAll('div.alert-radio-group button');
 
      // Determine whether the obtained element is not null
      if (!buttonElements.length || buttonElements.length !== this.flags.length) {   
        return;
      } else {
        // If it is not empty, loop through the obtained html element (that is, the html element where the information of the AlertController list is traversed)
        for (let index = 0; index < buttonElements.length; index++) {
          // According to the subscript to take the html element
          let buttonElement = buttonElements[index];
 
          // Then take the information in the list according to the html element
          let optionLabelElement = buttonElement.querySelector('.alert-radio-label');
          let flag = this.flags[index];

          // Splice the picture name to display the picture, pay attention to the picture naming, must be consistent with the binding field, then add Image for this element   
          optionLabelElement.innerHTML += '<ion-img src="assets/icon/' + flag + '.svg" style="width:20px;height:20px;float:right;margin-right: 15px;"></ion-image>';
        }
      }
    }, 60);
  }
}
