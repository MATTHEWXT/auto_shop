import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { NotificationService } from '../../../Core/notification/notification.service';

@Component({
  selector: 'app-creater-category',
  templateUrl: './creater-category.component.html',
  styleUrl: './creater-category.component.css'
})

export class CreaterCategoryComponent {
  category = {
    name: '',
  };
  selectedFile: File | null = null;
  errorMessage: string = '';
  successMessage: string = ''; // Для успешного сообщения

  private apiUrl = 'http://localhost:5186';

  constructor(private http: HttpClient, private notificationService: NotificationService) { }

  onSubmitCategory() {
    this.errorMessage = '';
    if (!this.category.name) {
      this.errorMessage = 'Ввведите название категории'
      return
    }
    const formData = new FormData();
    formData.append('name', this.category.name);

    this.http.post(`${this.apiUrl}/category/create`, formData).subscribe({
      next: (response) => {
        this.errorMessage = '';
        this.category.name ='';
        this.notificationService.showSuccess('Категория успешно создана');
        console.log('Category created successfully', response);
      },
      error: (error) => {
        const errorMessage = error?.error?.message || 'Ошибка при создании категории';
        this.notificationService.showError(errorMessage)
        this.category.name ='';

        console.error('Произошла ошибка во время создания категории.', error);
      }
    });
  }
}