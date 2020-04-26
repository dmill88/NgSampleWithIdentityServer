import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { Constants } from '../constants';
import { Subject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthContext } from '../models/auth-context';
import { SimpleClaim } from './../models/simple-claim';

@Injectable()
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();

  public loginChanged: Observable<boolean> = this._loginChangedSubject.asObservable();
  public authContext: AuthContext = null;

  constructor(private _httpClient: HttpClient) {
    console.log(`AuthService constructor  redirect_uri: ${Constants.clientRoot}signout-callback    post_logout_redirect_uri: ${Constants.clientRoot}signout-callback     silent_redirect_uri: ${Constants.clientRoot}assets/silent-callback.html`);

    const stsSettings = {
      authority: Constants.stsAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}signin-callback`,
      scope: 'openid profile projects-api',
      response_type: 'code',
      post_logout_redirect_uri: `${Constants.clientRoot}signout-callback`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${Constants.clientRoot}silent-callback.html`
      // metadata: {
      //   issuer: `${Constants.stsAuthority}`,
      //   authorization_endpoint: `${Constants.stsAuthority}authorize?audience=projects-api`,
      //   jwks_uri: `${Constants.stsAuthority}.well-known/jwks.json`,
      //   token_endpoint: `${Constants.stsAuthority}oauth/token`,
      //   userinfo_endpoint: `${Constants.stsAuthority}userinfo`,
      //   end_session_endpoint: `${Constants.stsAuthority}v2/logout?client_id=${Constants.clientId}&returnTo=${encodeURI(Constants.clientRoot)}signout-callback`
      // }
    };
    this._userManager = new UserManager(stsSettings);
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
    this._userManager.events.addUserLoaded(user => {
      if (this._user !== user) {
        this._user = user;
        this.loadSecurityContext();
        this._loginChangedSubject.next(!!user && !user.expired);
      }
    });

  }

  public login() {
    return this._userManager.signinRedirect();
  }

  public isLoggedIn(): Promise<boolean> {
    console.log('AuthService.isLoggedIn');
    return this._userManager.getUser().then(user => {
      const loggedIn = !!user && !user.expired;
      //if (this._user !== user) {
      //  this._loginChangedSubject.next(loggedIn);
      //}
      if (loggedIn && !this.authContext) {
        this.loadSecurityContext();
      }
      this._user = user;
      return loggedIn;
    });
  }

  public completeLogin() {
    return this._userManager.signinRedirectCallback().then(user => {
      this._user = user;
      this._loginChangedSubject.next(!!user && !user.expired);
      return user;
    });
  }

  public logout() {
    this._userManager.signoutRedirect();
  }

  public completeLogout() {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  }

  public getAccessToken() {
    return this._userManager.getUser().then(user => {
      if (!!user && !user.expired) {
        return user.access_token;
      }
      else {
        return null;
      }
    });
  }

  public loadSecurityContext(): void {
    console.log('Called loadSecurityContext');
    this._httpClient.get<AuthContext>(`${Constants.apiRoot}AuthContext`).subscribe(context => {
      let newAuthContext: AuthContext = new AuthContext();
      //newAuthContext.claims.push(context.claims);
      newAuthContext.claims = context.claims;
      newAuthContext.isAuthenticated = context.isAuthenticated;
      newAuthContext.name = context.name;
      newAuthContext.userId = context.userId;
      this.authContext = newAuthContext;
        }, error => console.error(error)
      );
  }

}

