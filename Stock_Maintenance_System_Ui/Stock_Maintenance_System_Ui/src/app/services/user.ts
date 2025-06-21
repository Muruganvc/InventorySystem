import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Api } from '../shared/services/api';
import { ApiResponse } from '../shared/common/ApiResponse';
import { UserCreateRequest } from '../models/User/UserCreateRequest';
import { UpdateUserRequest, ChangePasswordRequest } from '../models/User/UpdateUserRequest';
import { MenuItem } from '../models/User/LoginRequest';
import { UserList } from '../models/User/UserList';

@Injectable({
  providedIn: 'root',
})
export class User {
  private readonly httpApi = inject(Api);

  createUser = (user: UserCreateRequest): Observable<number> => {
    return this.httpApi
      .post<UserCreateRequest, number>('new-user', user)
      .pipe(map((res: ApiResponse<number>) => res.data));
  };

  updateUser = (userId: number, user: UpdateUserRequest): Observable<boolean> => {
    return this.httpApi
      .put<UpdateUserRequest, boolean>(`update/${userId}`, user)
      .pipe(map((res: ApiResponse<boolean>) => res.data));
  };

  changePassword = (userId: number, user: ChangePasswordRequest): Observable<boolean> => {
    return this.httpApi
      .put<ChangePasswordRequest, boolean>(`password-change/${userId}`, user)
      .pipe(map((res: ApiResponse<boolean>) => res.data));
  };

  getUsers = (): Observable<UserList[]> => {
    return this.httpApi
      .get<UserList[]>('users')
      .pipe(map((res: ApiResponse<UserList[]>) => res.data));
  };

  getUserMenu = (userId: number): Observable<MenuItem[]> => {
    return this.httpApi
      .get<MenuItem[]>(`menus/${userId}`)
      .pipe(map((res: ApiResponse<MenuItem[]>) => res.data));
  };

  getAllMenu = (): Observable<MenuItem[]> => {
    return this.httpApi
      .get<MenuItem[]>(`menus`)
      .pipe(map((res: ApiResponse<MenuItem[]>) => res.data));
  };
}
