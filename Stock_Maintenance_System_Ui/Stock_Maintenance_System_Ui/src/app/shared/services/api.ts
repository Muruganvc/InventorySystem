import { inject, Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiResponse } from '../common/ApiResponse';
import { APP_CONFIG } from '../common/app-config';

export type ApiHeaders = Record<string, string>;
export type ApiParams = Record<string, string | number | boolean | Array<string | number | boolean>>;

@Injectable({ providedIn: 'root' })
export class Api {
  private readonly config = inject(APP_CONFIG);
  private readonly http = inject(HttpClient);

  // Create and return HttpHeaders with optional custom headers
  private createHeaders(headers?: ApiHeaders): HttpHeaders {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

    if (headers) {
      Object.entries(headers).forEach(([key, value]) => {
        httpHeaders = httpHeaders.set(key, value);
      });
    }

    return httpHeaders;
  }

  // Convert object to HttpParams, handling array values as repeated params
  private createParams(params?: ApiParams): HttpParams {
    let httpParams = new HttpParams();

    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (Array.isArray(value)) {
          value.forEach(val => {
            httpParams = httpParams.append(key, String(val));
          });
        } else {
          httpParams = httpParams.set(key, String(value));
        }
      });
    }

    return httpParams;
  }

  // Handle HTTP errors globally
  private handleError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => error);
  }

  // GET request with optional query parameters and headers
  get<TResponse>(
    url: string,
    params?: ApiParams,
    headers?: ApiHeaders
  ): Observable<ApiResponse<TResponse>> {
    return this.http
      .get<ApiResponse<TResponse>>(`${this.config.apiBaseUrl}${url}`, {
        headers: this.createHeaders(headers),
        params: this.createParams(params),
      })
      .pipe(catchError(this.handleError));
  }

  // POST request with body, optional query parameters and headers
  post<TRequest, TResponse>(
    url: string,
    body: TRequest,
    params?: ApiParams,
    headers?: ApiHeaders
  ): Observable<ApiResponse<TResponse>> {
    return this.http
      .post<ApiResponse<TResponse>>(`${this.config.apiBaseUrl}${url}`, body, {
        headers: this.createHeaders(headers),
        params: this.createParams(params),
      })
      .pipe(catchError(this.handleError));
  }

  // PUT request with body, optional query parameters and headers
  put<TRequest, TResponse>(
    url: string,
    body: TRequest,
    params?: ApiParams,
    headers?: ApiHeaders
  ): Observable<ApiResponse<TResponse>> {
    return this.http
      .put<ApiResponse<TResponse>>(`${this.config.apiBaseUrl}${url}`, body ?? {}, {
        headers: this.createHeaders(headers),
        params: this.createParams(params),
      })
      .pipe(catchError(this.handleError));
  }

  // PATCH request for partial updates
  patch<TRequest, TResponse>(
    url: string,
    body: TRequest,
    params?: ApiParams,
    headers?: ApiHeaders
  ): Observable<ApiResponse<TResponse>> {
    return this.http
      .patch<ApiResponse<TResponse>>(`${this.config.apiBaseUrl}${url}`, body, {
        headers: this.createHeaders(headers),
        params: this.createParams(params),
      })
      .pipe(catchError(this.handleError));
  }

  // DELETE request with optional query parameters and headers
  delete<TResponse>(
    url: string,
    params?: ApiParams,
    headers?: ApiHeaders
  ): Observable<ApiResponse<TResponse>> {
    return this.http
      .delete<ApiResponse<TResponse>>(`${this.config.apiBaseUrl}${url}`, {
        headers: this.createHeaders(headers),
        params: this.createParams(params),
      })
      .pipe(catchError(this.handleError));
  }
}
