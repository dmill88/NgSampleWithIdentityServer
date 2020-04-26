import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Constants } from '../constants';
import { SharedModule } from './shared.module';
import { AuthContext } from '../models/auth-context';

@Injectable()
export class AccountService {
  userProfile: AuthContext;
  constructor(private _httpClient: HttpClient) { }

  public getAllUsers(): Observable<AuthContext[]> {
    return this._httpClient.get<AuthContext[]>(Constants.apiRoot + 'Account/Users');
  }

  public createUserProfile(userProfile: AuthContext) {
    return this._httpClient.post(`${Constants.apiRoot}Account/Profile`, userProfile);
  }

  public updateUserProfile(userProfile: AuthContext) {
    return this._httpClient.put(`${Constants.apiRoot}Account/Profile/${userProfile.userId}`, userProfile);
  }

  public register(userInfo: any) {
    return this._httpClient.post(`${Constants.apiRoot}Account/Register`, userInfo);
  }

}
