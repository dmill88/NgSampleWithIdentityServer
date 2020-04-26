import { Component } from '@angular/core';
import { AuthService } from '../shared/auth-service.component';
import { Constants } from './../constants';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  public isExpanded: boolean = false;
  public isLoggedIn: boolean = false;
  public isBlogEditor: boolean = false;

  constructor(
    private _authService: AuthService
  ) {
  }

  ngOnInit() {
    this._authService.loginChanged.subscribe(isLoggedIn => {
      //debugger;
      this.isLoggedIn = isLoggedIn;
      if (this._authService.authContext) {
        this.updateLoginStatus();
      }
    });
    if (this._authService.authContext) {
      console.log('ssssssssssssssssssss');
      this.updateLoginStatus();
    }
  }

  public login(): void {
    this._authService.login();
  }
  public logout(): void {
    return this._authService.logout();
  }

  public collapse(): void {
    this.isExpanded = false;
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  private updateLoginStatus(): void {
    this._authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
      this.isBlogEditor = this._authService.authContext.isInRole(Constants.userRoles.blogEditor);
    })
  }
}
