import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertController, LoadingController } from '@ionic/angular';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor  implements HttpInterceptor {
  private readonly UNKNOWN_ERROR = 'Something went wrong';

  constructor(private loadingCntrl: LoadingController, private alertCntrl: AlertController){}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(0),
      catchError(err => {
        const errorDescription = err.error.description ? err.error.description : err.error;
        let message = '';

        if (err.status === 400) {
            if (!errorDescription) {
                message = this.UNKNOWN_ERROR;
            } else {
                message = errorDescription;
            }
        } else {
          message = this.UNKNOWN_ERROR;
        }

        // reset all overlays
        this.loadingCntrl.dismiss().then();

        this.alertCntrl.create({
          header: 'Error', 
          message: message,
          buttons: ['Ok']
        }).then(el => el.present());

        return throwError(err);
      }));
  }
}
