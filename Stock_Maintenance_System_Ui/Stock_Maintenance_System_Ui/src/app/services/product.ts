import { inject, Injectable } from '@angular/core';
import { Api } from '../shared/services/api';
import { ProductResponse } from '../models/Product/ProductResponse';
import { KeyValuePair } from '../shared/common/KeyValuePair';
import { ProductRequest } from '../models/Product/ProductRequest';
import { ProductsResponse } from '../models/Product/ProductsResponse';
import { UpdateProductRequest } from '../models/Product/UpdateProductRequest';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../shared/common/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class Product {
  private readonly httpApi = inject(Api);

  getProduct = (companyId: number): Observable<ProductResponse[]> => {
    return this.httpApi
      .get<ProductResponse[]>(`product-company?companyId=${companyId}`)
      .pipe(map((res: ApiResponse<ProductResponse[]>) => res.data));
  }

  getCompany = (companyName?: string): Observable<KeyValuePair[]> => {
    const url = companyName
      ? `companyName=${encodeURIComponent(companyName)}`
      : `company`;
    return this.httpApi
      .get<KeyValuePair[]>(url)
      .pipe(map((res: ApiResponse<KeyValuePair[]>) => res.data));
  }

  getCategories = (companyId: number): Observable<KeyValuePair[]> => {
    return this.httpApi
      .get<KeyValuePair[]>(`category/${companyId}`)
      .pipe(map((res: ApiResponse<KeyValuePair[]>) => res.data));
  }

  getProductCategories = (categoryId: number): Observable<KeyValuePair[]> => {
    return this.httpApi
      .get<KeyValuePair[]>(`product-category/${categoryId}`)
      .pipe(map((res: ApiResponse<KeyValuePair[]>) => res.data));
  }

  createProduct = (product: ProductRequest): Observable<number> => {
    return this.httpApi
      .post<ProductRequest, number>('product', product)
      .pipe(map((res: ApiResponse<number>) => res.data));
  }

  getProductTypes = (): Observable<ProductsResponse[]> => {
    return this.httpApi
      .get<ProductsResponse[]>('products')
      .pipe(map((res: ApiResponse<ProductsResponse[]>) => res.data));
  }

  updateProduct = (productId: number, product: UpdateProductRequest): Observable<number> => {
    return this.httpApi
      .put<UpdateProductRequest, number>(`product/${productId}`, product)
      .pipe(map((res: ApiResponse<number>) => res.data));
  }

  activatProduct = (productId: number): Observable<number> => {
    return this.httpApi
      .put<{}, number>(`product/activate/${productId}`, {})
      .pipe(map((res: ApiResponse<number>) => res.data));
  }
}
