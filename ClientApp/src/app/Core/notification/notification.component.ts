import { Component } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent {
  messageError: string | null = null;
  messageSuccess: string | null = null;
  isShowAlertMessage: boolean = false;

private fadeTimeout: any; 
private displayTimeout: any; 

showError(message: string) {
  this.clearTimers(); 
  this.messageError = message;
  this.isShowAlertMessage = true;
  this.displayTimeout = setTimeout(() => {
    this.fadeOut();
  }, 4000);
}

showSuccess(message: string) {
  this.clearTimers(); 
  this.messageSuccess = message;
  this.isShowAlertMessage = true;
  this.displayTimeout = setTimeout(() => {
    this.fadeOut();
  }, 4000);
}

private fadeOut() {
  this.isShowAlertMessage = true;

  this.fadeTimeout = setTimeout(() => {
    this.isShowAlertMessage = false;
    this.deleteAlert()
  }, 500);
}

private deleteAlert(){
  this.fadeTimeout = setTimeout(() => {
    this.messageError = null;
    this.messageSuccess = null;
  }, 500);
}

private clearTimers() {
  if (this.displayTimeout) {
    clearTimeout(this.displayTimeout);
  }
  if (this.fadeTimeout) {
    clearTimeout(this.fadeTimeout);
  }
}
}
