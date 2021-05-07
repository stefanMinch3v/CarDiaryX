import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {

  constructor(private modalCntrl: ModalController) { }

  ngOnInit() {}

  onDismissModal(): void {
    this.modalCntrl.dismiss({ 'dismissed': true });
  }

  async onSubmit(form: NgForm): Promise<void> {
    console.log(form);
  }
}
