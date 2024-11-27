import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket.service';
import { AuthService } from '../../Core/Services/auth.service';
import { Router } from '@angular/router';
import { CheckoutService } from './checkout/checkout.service';
import { NotificationService } from '../../Core/notification/notification.service';

export interface BasketItem {
  id: string; 
  UnitPrice: number; 
  product: Product;
  selected: boolean;
}

export interface Product {
  id: string; 
  name: string; 
  price: number; 
  imagePath: string;
}

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})

export class BasketComponent implements OnInit {
  isLoading: boolean = true;

  isLoggedIn = false;
  userId: string = '';
  basketItems: BasketItem[] = [];
  baseUrl: string = 'http://localhost:5186';

  totalPrice: number = 0; 
  totalCount: number = 0;

  constructor(private basketService: BasketService, private authService: AuthService, private router: Router, private checkoutService: CheckoutService, private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.userId = this.authService.getUserId() ?? '';
    this.loadBasket();
  }

  loadBasket(): void {
    if (this.isLoggedIn) {
      this.basketService.getBasket(this.userId).subscribe(
        (data) => {
          this.basketItems = data;
          this.isLoading = false;
        },
        (error) => {
          const errorMessage = error?.error?.message || 'Произошла ошибка при загрузке корзины';
          this.notificationService.showError(errorMessage);
          console.error('Error loading basket', error);
          this.isLoading = false;
        }
      );
    }
    else{
      this.isLoading = false;
    }
  }

  get selectedTotalPrice(): number {
    return this.basketItems
      .filter(item => item.selected)
      .reduce((total, item) => total + item.product.price, 0);
  }

  get selectedItemCount(): number {
    return this.basketItems
      .filter(item => item.selected)
      .length;
  }

  anyItemSelected(): boolean {
    return this.basketItems.some(item => item.selected);
  }

  goToCheckOut(): void {
    const selectedItems = this.basketItems.filter(item => item.selected);
    this.router.navigate(['/basket/checkout']);
    this.checkoutService.setSelectedItems(selectedItems);
  }
}
