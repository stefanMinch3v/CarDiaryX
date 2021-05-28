import { HttpHandler, HttpEvent, HttpRequest, HttpInterceptor } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
  
@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
      req: HttpRequest<any>,
      next: HttpHandler
    ): Observable<HttpEvent<any>> {
    return from(this.handle(req, next));
  }

  private async handle(req: HttpRequest<any>, next: HttpHandler) {
    try {
      const result = await this.authService.getToken();

      if (result && result.value) {
        req = req.clone({
          headers: req.headers.set('Authorization', 'Bearer ' + result.value)
        });
      }
    } catch (error) {
      console.log(error);
    }

    return next.handle(req).toPromise();
  }
}