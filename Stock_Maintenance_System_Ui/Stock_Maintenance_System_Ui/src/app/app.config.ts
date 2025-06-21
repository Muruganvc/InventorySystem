import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
  provideBrowserGlobalErrorListeners,
  ErrorHandler,
} from '@angular/core';
import {
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi
} from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import { routes } from './app.routes';
import { TokenInterceptor } from './Interceptors/TokenInterceptor';
import { LoaderInterceptor } from './Interceptors/LoaderInterceptor';
import { ErrorInterceptor } from './Interceptors/ErrorInterceptor'; // 🔴 Custom Interceptor
import { APP_CONFIG, appConfigValues } from './shared/common/app-config';
import { GlobalErrorHandler } from './shared/common/global-error-handler';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    { provide: APP_CONFIG, useValue: appConfigValues },
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    provideHttpClient(
      withInterceptors([
        LoaderInterceptor,
        TokenInterceptor,
        ErrorInterceptor // ✅ Add this
      ])
    ),
    provideHttpClient(withInterceptorsFromDi()),
    importProvidersFrom(
      ToastrModule.forRoot({
        positionClass: 'toast-bottom-right',
        timeOut: 3000,
        preventDuplicates: true,
      })
    )
  ]
};
