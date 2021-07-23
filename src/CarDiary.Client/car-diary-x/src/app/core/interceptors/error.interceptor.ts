import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController, LoadingController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor  implements HttpInterceptor {
  private readonly UNKNOWN_ERROR = 'An unexpected server error has occurred. Our team has been notified.';
  private readonly DUPLICATE_USERNAME_MESSAGE = 'is already taken.';
  private readonly SESSION_EXPIRED = 'Your session has expired.';
  private readonly UNEXPECTED_SERVER_ERROR = 'An unexpected error has occurred. Please try again later.';

  constructor(
    private loadingCntrl: LoadingController, 
    private alertCntrl: AlertController,
    private translate: TranslateService,
    private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(0),
      catchError(err => {
        // reset all overlays
        this.loadingCntrl.getTop().then(overlay => overlay ? overlay.dismiss() : null);

        const messages = this.parseErrorData(err);

        const translated =  this.formatAndTranslateMessages(messages);

        this.alertCntrl.create({
          header: translated[0], 
          message: translated[1],
          buttons: [translated[2]]
        }).then(el => el.present());

        if (messages.some(m => m === this.SESSION_EXPIRED)) {
          this.router.navigate(['auth']);
        }

        return throwError(err);
      }));
  }

  // add custom messages here that come in format Abc 'specific name' defgh
  private formatAndTranslateMessages(messages: Array<string>): Array<string> {
    const translated = [];

    for (const message of messages) {
      if ([...this.indexOfSubstrings(message, this.DUPLICATE_USERNAME_MESSAGE)].length > 0) {
        const email = message.match(/'([^']+)'/)[1]; // cut 'email' out of full sentence

        translated.push(this.translate.instant(['Error', 'Email is already taken', 'Ok'], { value: email }));
      } else {
        translated.push(this.translate.instant(['Error', message, 'Ok']));
      }
    }

    const result = [];

    for (const translate of translated) {
      for (const [key, value] of Object.entries(translate)) {
        result.push(value);
      }
    }

    return result;
  }

  private parseErrorData(err: any): Array<string> {
    let errorMessages: Array<string> = [];

    if (err.status === 400) {
      if (!err.error?.errors) {
        errorMessages = [this.UNEXPECTED_SERVER_ERROR];
      } else {
        errorMessages = err.error?.errors;
      }
    } else if (err.status === 401) {
      errorMessages = [this.SESSION_EXPIRED];
    } else {
      errorMessages = [this.UNKNOWN_ERROR];
    }

    return errorMessages;
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
