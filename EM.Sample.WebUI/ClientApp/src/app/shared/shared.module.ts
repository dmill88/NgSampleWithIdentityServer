import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './auth-interceptor.service';
import { AuthService } from './auth-service.component';
import { AccountService } from './account.service';
import { AdminRouteGuard } from './admin-route-guard';
import { RoleRouteGuard } from './role-route-gaurd';

@NgModule({
  imports: [],
  exports: [],
  declarations: [],
  providers: [
    AuthService,
    AccountService,
    AdminRouteGuard,
    RoleRouteGuard,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
  ],
})
export class SharedModule { }
