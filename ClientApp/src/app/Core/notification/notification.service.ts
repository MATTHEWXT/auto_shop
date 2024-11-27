import { Injectable } from '@angular/core';
import { NotificationComponent } from './notification.component';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notificationComponent!: NotificationComponent;

  registerNotificationComponent(component: NotificationComponent) {
    this.notificationComponent = component;
  }

  showError(message: string) {
    this.notificationComponent?.showError(message);
  }

  showSuccess(message: string) {
    this.notificationComponent?.showSuccess(message);
  }
}
