import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationService } from '../Core/notification/notification.service';

export interface Product {
  id: string;
  name: string;
  price: number;
  imagePath: string;
  categoryId: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5186';
  basketItem = {
    userId: '',
    productId: ''
  };

  constructor(private http: HttpClient, private notificationService: NotificationService) { }

  getProductsByCategory(name: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/cache/getProductsByCategory/${name}`);
  }

  getAFewProductsForHome(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/product/getAFewProductsForHome`);
  }

  getProductsBySearchTerm(searchTerm: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/product/catalog/${searchTerm}`);
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/product/id/${id}`);
  }

  getProductByName(name: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/product/name/${name}`);
  }

  updateProduct(formData: FormData): void {
    this.http.post(`${this.apiUrl}/product/update`, formData).subscribe(response => {
      this.notificationService.showSuccess('Товар успешно изменен');
      console.log('Товар успешно изменен', response);
    }, error => {
      console.error('Ошибка при изменении товара', error);
      const errorMessage = error?.error?.message || 'Ошибка при изменении товара';
      this.notificationService.showError(errorMessage);
    });
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/product/delete/${id}`);
  }

  addProductToBasket(userId: string, productId: string) {
    this.basketItem.userId = userId;
    this.basketItem.productId = productId;

    this.http.post(`${this.apiUrl}/basket`, this.basketItem).subscribe(response => {
      console.log('Product added to basket successfully', response);
    }, error => {
      console.error('Error added to basket product', error);
    });
  }
}
