import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth-service.component';

@Injectable()
export class RoleRouteGuard implements CanActivate {
  constructor(
    private _authService: AuthService,
    private _router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let allowed = false;
    if (route.data == null || route.data.role == null) {
      allowed = true; // No role specified, so allow. 
    } else if (this._authService.authContext) {
      allowed = this._authService.authContext.isInRole(route.data.role);
    }
    console.log('RoleRouteGuard.canActivate', allowed);
    return allowed;
  }
}
