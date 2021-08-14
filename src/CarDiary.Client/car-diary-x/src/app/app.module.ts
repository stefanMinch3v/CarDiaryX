import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { SettingsComponent } from './components/settings/settings.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TokenInterceptor } from './core/interceptors/token.interceptor';
import { initSynchronousFactory } from './core/helpers/init-synchronous.factory';
import { SettingsService } from './core/services/settings.service';

@NgModule({
  declarations: [AppComponent, SettingsComponent],
  entryComponents: [],
  imports: [
    BrowserModule, 
    FormsModule,
    HttpClientModule,
    IonicModule.forRoot({
      backButtonText: '',
      swipeBackEnabled: false
    }), 
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    })],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initSynchronousFactory,
      deps: [SettingsService],
      multi: true
    },
    { 
      provide: RouteReuseStrategy, 
      useClass: IonicRouteStrategy 
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent],
})
export class AppModule { }

// AOT compilation support
export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
