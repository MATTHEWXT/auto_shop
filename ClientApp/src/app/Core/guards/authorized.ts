import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../Services/auth.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../notification/notification.service';
@Injectable({
  providedIn: 'root', 
})
export class authorizedGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const isauthorized = this.authService.isLoggedIn(); 
    if (!isauthorized) {
      this.notificationService.showError("Для доступа нужно авторизоваться")
    }
    return isauthorized;
  }
}