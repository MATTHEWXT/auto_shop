import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../product.service';
import { AuthService } from '../../Core/Services/auth.service';
import { NotificationService } from '../../Core/notification/notification.service';
import localeRu from '@angular/common/locales/ru';
import { registerLocaleData } from '@angular/common';

registerLocaleData(localeRu, 'ru');

@Component({
  selector: 'app-search-product',
  templateUrl: './search-product.component.html',
  styleUrl: './search-product.component.css'
})
export class SearchProductComponent implements OnInit {
baseUrl = 'http://localhost:5186';

searchTerm: string = '';
products: any[] = [];
userId: string = '';

isAddedToBasket: { [key: string]: boolean } = {};
isLoggedIn = false;
isLoading = true;

  constructor(private router: Router, private route: ActivatedRoute, private productService: ProductService, private authService: AuthService, private notificationService: NotificationService) { }
  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {
      this.searchTerm = params.get('searchTerm') || '';
      this.productService.getProductsBySearchTerm(this.searchTerm).subscribe(data => {
        this.products = data;
      });
    });

    this.isLoggedIn = this.authService.isLoggedIn();
    this.isLoading = false;

  }

  addProductToBasket(productId: string): void {
    if(this.isAddedToBasket[productId]){
      this.router.navigate(['/basket']);
    }
    this.isAddedToBasket[productId] = !this.isAddedToBasket[productId]; 
    if (this.isLoggedIn) {
      this.userId = this.authService.getUserId() ?? '';
      this.productService.addProductToBasket(this.userId, productId);
    }
    else {
      this.notificationService.showError('Необходимо авторизоваться, чтобы добавить товар в корзину')
    }
  }
}
