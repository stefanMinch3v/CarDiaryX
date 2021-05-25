import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastController: ToastController, private translateService: TranslateService) { }

  async presentSuccessToast(message?: string): Promise<void> {
    const toast = await this.toastController.create({
      message: this.translateService.instant(message ? message : 'Success'),
      duration: 2000,
      color: 'success'
    });
    toast.present();
  }
}
