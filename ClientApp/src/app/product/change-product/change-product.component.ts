import { Component, OnInit } from '@angular/core';
import { Product, ProductService } from '../product.service';
import { CategoryService } from '../category/category.service';
import { NotificationService } from '../../Core/notification/notification.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-change-product',
  templateUrl: './change-product.component.html',
  styleUrl: './change-product.component.css'
})
export class ChangeProductComponent implements OnInit {
  baseUrl = 'http://localhost:5186';

  categories: any[] = [];

  product: Product | undefined;
  errorMessage: string | null = null;
  errorMessageCreater: string | null = null;
  productSearchCriteria: string = '';
  selectedFile: File | null = null;
  selectedImageUrl: string | null = null;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private notificationService: NotificationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe((data: any) => {
      this.categories = data.categories;
    });
  }

  findProduct(): void {
    if (!this.productSearchCriteria) {
      this.errorMessage = 'Введите ID или название продукта'
      return
    }
    if (this.productSearchCriteria.length === 36) {
      this.productService.getProductById(this.productSearchCriteria.toLowerCase()).subscribe(
        (product) => {
          this.product = product;
          this.errorMessage = null;
        },
        (error) => {
          const errorMessage = error?.error?.message || 'Продукт с указанным ID не найден';
          this.notificationService.showError(errorMessage);
        }
      );
    }
    else {
      this.productService.getProductByName(this.productSearchCriteria.toLowerCase()).subscribe(
        (product) => {
          this.product = product;
          this.errorMessage = null;
        },
        (error) => {
          const errorMessage = error?.error?.message || 'Продукт с указанным именем не найден';
          this.notificationService.showError(errorMessage);
        }
      );
    }
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

  onSubmit(): void {
    if (!this.product?.name) {
      this.errorMessageCreater = 'Введите название';
      return
    }

    else if (!this.product?.categoryId) {
      this.errorMessageCreater = 'Выберите категорию товара';
      return
    }

    else if (!this.product?.price) {
      this.errorMessageCreater = 'Цена не может быть 0';
      return
    }

    else if (!this.product?.imagePath) {
      this.errorMessageCreater = 'Выберите изображение';
      return
    }
    const formData = new FormData();
    if (this.product) {
      formData.append('id', this.product.id);
      formData.append('name', this.product.name.toLowerCase());
      formData.append('price', this.product.price.toString());
      formData.append('categoryId', this.product.categoryId)
    }
    if (this.selectedFile) {
      formData.append('imageFile', this.selectedFile, this.selectedFile.name);
    }
    this.productService.updateProduct(formData);
    this.errorMessageCreater = '';
  }

  deleteProduct(): void {
    if (this.product?.id) {
      this.productService.deleteProduct(this.product.id).subscribe(
        () => {
          console.log('Товар успешно удален');
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate(['/product-change']);
            this.notificationService.showSuccess('Товар успешно удален');
            this.errorMessageCreater = '';
          });
        },
        (error) => {
          const errorMessage = error?.error?.message || 'Ошибка при удалении товара';
          this.notificationService.showError(errorMessage);
          console.error('Ошибка при удалении товара:', error)
        }
      );
    }
  }
}
