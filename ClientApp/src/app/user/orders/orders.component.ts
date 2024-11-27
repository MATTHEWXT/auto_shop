import { Component, OnInit } from '@angular/core';
import { OrdersService } from './orders.service';
import { AuthService } from '../../Core/Services/auth.service';
import { NotificationService } from '../../Core/notification/notification.service';

export interface Order {
  id: string; 
  customerId: string; 
  date: Date;
  status: string;
  shippedDate: string;
  totalAmount: number;
  orderItems: OrderItem[];
}

export interface OrderItem {
  id: string; 
  OrderId: string;
  productId: string;
  product: Product
  UnitPrice: number;
}

export interface Product {
  id: string; 
  categoryId:string;
  imagePath: string;
  name: string;
  price: number;
}

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit{
  baseUrl = 'http://localhost:5186';

  isLoggedIn = false;
  isLoading = true;

  userId: string ='';

  orders: Order[] = [];

  constructor(private ordersService: OrdersService, private authService: AuthService, private notificationService: NotificationService) {}
    
  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.userId = this.authService.getUserId() ?? '';
    this.loadOrders();
  }

  loadOrders(): void {
    if (this.isLoggedIn) {
      this.ordersService.getUserOrders(this.userId).subscribe(
        (data) => {
          this.orders = data;
          this.isLoading = false;
        },
        (error) => {
          const errorMessage = error?.error?.message || 'Произошла ошибка при загрузке заказов';
          this.notificationService.showError(errorMessage);
          console.error('Error loading orders', error);
          this.isLoading = false;
        }
      );
    }
    else{
      this.isLoading = false;
    }
  }
}
