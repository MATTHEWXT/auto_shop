import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CategoryService } from '../category/category.service';
import { NotificationService } from '../../Core/notification/notification.service';


@Component({
  selector: 'app-creater-product',
  templateUrl: './creater-product.component.html',
  styleUrl: './creater-product.component.css'
})
export class CreaterProductComponent implements OnInit {
  product = {
    name: '',
    categoryId: '',
    price: 0,
  };

  selectedFile: File | null = null;
  selectedImageUrl: string | null = null;
  categories: any[] = [];

  errorMessage: string ='';

  isLoading = true;
  private apiUrl = 'http://localhost:5186';

  constructor(private http: HttpClient, private categoryService: CategoryService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe((data: any) => {
      this.categories = data.categories;
      this.isLoading = false;

    });
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files[0]) {
      const file = fileInput.files[0];
      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedImageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmitProduct() {
    if(!this.product.name){
      this.errorMessage = 'Введите название';
      return
    }

    else if(!this.product.categoryId){
      this.errorMessage = 'Выберите категорию товара';
      return
    }

    else if(!this.product.price){
      this.errorMessage = 'Цена не может быть 0';
      return
    }

    else if(!this.selectedFile){
      this.errorMessage = 'Выберите изображение';
      return
    }

    const formData = new FormData();
    formData.append('name', this.product.name.toLowerCase());
    formData.append('categoryId', this.product.categoryId);
    formData.append('price', this.product.price.toString());

    if (this.selectedFile) {
      formData.append('imageFile', this.selectedFile, this.selectedFile.name);
    }

    this.http.post(`${this.apiUrl}/product/create`, formData).subscribe(response => {
      this.notificationService.showSuccess('Продукт успешно создан');
      console.log('Product created successfully', response);
      this.errorMessage = '';
      this.product.name = '';
      this.product.price = 0;
      this.product.categoryId = '';
     
      this.selectedFile = null;
    }, error => {
      const errorMessage = error?.error?.message || 'Не удалось создать товар';
      this.notificationService.showError(errorMessage);
      console.error('Error creating product', error);
    });
  }
}
