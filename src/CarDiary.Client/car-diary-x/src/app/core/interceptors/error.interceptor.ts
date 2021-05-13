import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertController, LoadingController } from '@ionic/angular';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor  implements HttpInterceptor {
  private readonly UNKNOWN_ERROR = 'Server is not responding...';

  constructor(
    private loadingCntrl: LoadingController, 
    private alertCntrl: AlertController) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(0),
      catchError(err => {
        let errorDescription = '';

        if (err.error.description) {
          errorDescription = err.error.description;
        } else if (err.error.errors) {
          for (const [key, value] of Object.entries(err.error.errors)) {
            errorDescription += `${value}\n`;
          }
        } else {
          errorDescription = err.error;
        }

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
        this.loadingCntrl.getTop().then(overlay => overlay ? overlay.dismiss() : null);
        
        this.alertCntrl.create({
          header: 'Error', 
          message: message,
          buttons: ['Ok']
        }).then(el => el.present());

        return throwError(err);
      }));
  }
}
