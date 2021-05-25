import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertController, LoadingController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor  implements HttpInterceptor {
  private readonly UNKNOWN_ERROR = 'Server is not responding...';
  private readonly DUPLICATE_USERNAME_MESSAGE = 'is already taken.';

  constructor(
    private loadingCntrl: LoadingController, 
    private alertCntrl: AlertController,
    private translate: TranslateService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(0),
      catchError(err => {
        // reset all overlays
        this.loadingCntrl.getTop().then(overlay => overlay ? overlay.dismiss() : null);

        const message = this.parseErrorData(err);

        const translated =  this.formatAndTranslateMessages(message);

        this.alertCntrl.create({
          header: translated[0], 
          message: translated[1],
          buttons: [translated[2]]
        }).then(el => el.present());

        return throwError(err);
      }));
  }

  // add custom messages here that come in format Abc 'specific name' defgh
  private formatAndTranslateMessages(message: string): string[] {
    message = message.toString(); // sometimes gets ['abc'] instead of 'abc'

    let translated = [];

    if ([...this.indexOfSubstrings(message, this.DUPLICATE_USERNAME_MESSAGE)].length > 0) {
      message = message.match(/'([^']+)'/)[1]; // cut 'username' out of full sentence

      translated = this.translate.instant(['Error', 'Username is already taken', 'Ok'], { value: message });
    } else {
      translated = this.translate.instant(['Error', message , 'Ok']);
    }

    let result = [];

    for (const [key, value] of Object.entries(translated)) {
      result.push(value);
    }

    return result;
  }

  private parseErrorData(err: any): string {
    let errorDescription = '';

    if (err?.error?.detail) {
      errorDescription = err.error.detail;
    } else {
      errorDescription = String(err.error);
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

    return message;
  }

  // https://github.com/30-seconds/30-seconds-of-code/blob/master/snippets/indexOfSubstrings.md
  private indexOfSubstrings = function* (str, searchValue) {
    let i = 0;
    while (true) {
      const r = str.indexOf(searchValue, i);
      if (r !== -1) {
        yield r;
        i = r + 1;
      } else return;
    }
  };
}
