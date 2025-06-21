import { InjectionToken } from '@angular/core';

export interface AppConfig {
  apiBaseUrl: string;
}

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export const appConfigValues: AppConfig = {
  apiBaseUrl: 'https://api.example.com/'
};
