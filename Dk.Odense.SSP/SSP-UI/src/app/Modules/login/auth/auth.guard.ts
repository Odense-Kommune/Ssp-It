import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { UserService } from '../service/user.service';
import { environment } from '../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class NeedAuthGuard implements CanActivate {
  constructor(private userService: UserService) {}

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.userService.isLoggedIn) return true;

    if (!sessionStorage.getItem('url')) {
      sessionStorage.setItem('url', window.location.pathname);
    }

    this.userService.isAuthorized();

    return this.userService.isLoggedIn;
  }
}
