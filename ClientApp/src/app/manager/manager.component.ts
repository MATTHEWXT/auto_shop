import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../user/orders/orders.service';
import { AuthService } from '../Core/Services/auth.service';
import { Order } from '../user/orders/orders.component';
import { ManagerService } from './manager.service';
import { Router } from '@angular/router';
import { OrderDetailService } from './order-detail/order-detail.service';

@Component({
  selector: 'app-manager',
  templateUrl: './manager.component.html',
  styleUrl: './manager.component.css'
})
export class ManagerComponent implements OnInit {
  baseUrl: string =  'http://localhost:5186';
  allOrders: Order[] = [];
  sentOrders: Order[] = [];
  selectedTab = 'newOrders';

  orderId: string = '';
  isLoggedIn = false;
  isLoading = true;

  constructor(private orderDetailService: OrderDetailService, private router: Router,private managerService: ManagerService, private ordersService: OrdersService, private authService: AuthService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.loadOrders();
  }

  loadOrders() {
    if (this.isLoggedIn) {
      this.ordersService.getAllOrders().subscribe(
        (data) => {
      this.allOrders = data.filter(order => order.status === 'Создано');
      this.sentOrders = data.filter(order => order.status === 'Отправлено');

      this.isLoading = false;
        },
        (error) => {
          console.error('Error loading basket', error);
          this.isLoading = false;
        }
      );
    }
    else{
      this.isLoading = false;
    }
  }
  goToOrderDetails(order: Order): void {
    this.router.navigate(['/orders-managment/order-details']);
    this.orderDetailService.setOrder(order)
  }

  selectTab(tab: string) {
    this.selectedTab = tab;
  }

}
