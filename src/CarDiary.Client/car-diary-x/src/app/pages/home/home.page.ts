import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { LanguageComponent } from 'src/app/components/language/language.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {

  constructor(private modalCntrl: ModalController) { }

  ngOnInit() {
  }

  async openModal(): Promise<void> {
    const modal = await this.modalCntrl.create({
      component: LanguageComponent,
      id: 'lang'
    });

    return await modal.present();
  }
}
