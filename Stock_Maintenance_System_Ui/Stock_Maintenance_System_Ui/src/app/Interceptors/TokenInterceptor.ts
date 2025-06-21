import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { Auth } from '../services/Auth';

export const TokenInterceptor: HttpInterceptorFn = (req, next) => {
    const tokenService = inject(Auth);
    const token = tokenService.getToken();
    console.log("TOken Interceptor triggered...")
    if (token) {
        const authReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        });
        return next(authReq);
    }

    return next(req);
};
