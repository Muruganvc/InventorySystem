import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { Loader } from '../services/loader';
export const LoaderInterceptor: HttpInterceptorFn = (req, next) => {
    const loader = inject(Loader);
    console.log('⏳ Interceptor triggered:', req.url);
    loader.show();
    return next(req).pipe(finalize(() => loader.hide()));
};
