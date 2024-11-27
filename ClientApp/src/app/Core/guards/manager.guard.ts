import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../Services/auth.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../notification/notification.service';
@Injectable({
  providedIn: 'root', 
})
export class ManagerGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const isManager = this.authService.hasRole('manager'); 
    if (!isManager) {
      this.router.navigate(['/']);
      this.notificationService.showError("Для доступа нужны права менеджера.")

    }
    return isManager;
  }
}