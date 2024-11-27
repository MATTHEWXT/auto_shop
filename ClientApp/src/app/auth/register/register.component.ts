import { Component } from '@angular/core';
import { AuthService } from '../../Core/Services/auth.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { NotificationService } from '../../Core/notification/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  firstName: string = '';
  email: string = '';
  password: string = '';
  repassword: string = '';
  errorMessage: string = '';

  isSabmitBtnLock = false;

  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) { }

  onRegister(registrationForm: NgForm) {
    if (!this.isSabmitBtnLock) {
      if (!this.firstName) {
        this.errorMessage = 'Введите имя';
        console.error(this.errorMessage);
        return;
      }
      else if (!this.email) {
        this.errorMessage = 'Введите email';
        console.error(this.errorMessage);
        return;
      }
      else if (!this.password) {
        this.errorMessage = 'Введите пароль';
        console.error(this.errorMessage);
        return;
      }
      else if (this.password !== this.repassword) {
        this.errorMessage = 'Пароли не совпадают!';
        console.error(this.errorMessage);
        return;
      }

      this.isSabmitBtnLock = true;
      this.authService.register(this.firstName, this.email, this.password)
        .subscribe({
          next: (response) => {
            this.authService.setTokens(response)
            console.log('Регистрация успешна', response);
            this.isSabmitBtnLock = false;
            this.router.navigate(['/']);
          },
          error: (error) => {
            const errorMessage = error?.error?.message || 'Ошибка регистрации';
            this.notificationService.showError(errorMessage);
            this.isSabmitBtnLock = false;
            console.error('Ошибка регистрации', error);
          }
        });
    }
  }

  goToLogin() {
    this.router.navigate(['/login']); // навигация на страницу логина
  }
}
