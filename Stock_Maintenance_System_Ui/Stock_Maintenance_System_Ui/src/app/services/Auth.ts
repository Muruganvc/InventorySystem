import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class Auth {
    getToken(): string | null {
        return localStorage.getItem('access_token'); // or from cookies/session
    }
}