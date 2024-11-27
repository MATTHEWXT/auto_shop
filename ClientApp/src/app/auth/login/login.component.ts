import { Component } from '@angular/core';
import { AuthService } from '../../Core/Services/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../Core/notification/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  isSabmitBtnLock = false;

  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) { }

  onSubmit() {
    if(!this.isSabmitBtnLock){
      if (!this.email) {
        this.errorMessage = 'Введите email';
        return;
      }
      if (!this.password) {
        this.errorMessage = 'Введите пароль';
        return;
      }

      this.isSabmitBtnLock = true;
      this.authService.login(this.email, this.password).subscribe({
        next: (response) => {
          if (response) {
            this.authService.setTokens(response)
            this.isSabmitBtnLock = false;
          }
          this.router.navigate(['/']);
        },
        error: (error) => {
          if (error.status === 401) {
            this.errorMessage = error?.error?.message;
            this.isSabmitBtnLock = false;
          }
          else {
            const errorMessage = error?.error?.message || 'Ошибка входа. Пожалуйста, проверьте свои учетные данные.';
            this.notificationService.showError(errorMessage);
            this.isSabmitBtnLock = false;
            console.error('Ошибка входа', error);
          }
        }
      });
    }
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}
