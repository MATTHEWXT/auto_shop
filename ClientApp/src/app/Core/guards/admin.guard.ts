import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../Services/auth.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../notification/notification.service';
@Injectable({
  providedIn: 'root', 
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const isAdmin = this.authService.hasRole('admin'); 
    if (!isAdmin) {
      this.router.navigate(['/']);
      this.notificationService.showError("Для доступа нужны права администратора.")
    }
    return isAdmin;
  }
}