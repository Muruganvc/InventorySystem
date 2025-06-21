// src/app/services/loader.ts
import { Injectable, Signal, computed, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class Loader {
  private _loading = signal(false);

  show() {
    this._loading.set(true);
  }

  hide() {
    this._loading.set(false);
  }

  readonly showSignal: Signal<boolean> = computed(() => this._loading());
}
