import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from '../../user/orders/orders.component';
import { CheckoutService } from '../../user/basket/checkout/checkout.service';
import { AuthService } from '../../Core/Services/auth.service';
import { OrderDetailService } from './order-detail.service';
import { NotificationService } from '../../Core/notification/notification.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css'
})
export class OrderDetailComponent implements OnInit {
  baseUrl: string = 'http://localhost:5186';

  order: Order | undefined;
  userId: string = '';

  customerId: string = '';
  firstName: string = '';
  lastName: string = '';
  phoneNumber: string = '';
  _addressObj: { city: string; street: string; house: string } | null = null;
  city: string = '';
  street: string = '';
  house: string = '';

  constructor(private orderDetailService: OrderDetailService, private router: Router, private checkoutService: CheckoutService, private authService: AuthService, private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.order = this.orderDetailService.getOrder();
    this.loadCustomer();
  }
  loadCustomer(): void {
    this.userId = this.authService.getUserId() ?? '';
    this.checkoutService.getCustomer(this.userId).subscribe(
      (data) => {
        this.customerId = data.id;
        this.firstName = data.firstName;
        this.lastName = data.lastName;
        this.phoneNumber = data.phoneNumber;
        this._addressObj = JSON.parse(data.address);

        this.city = this._addressObj?.city ?? '';
        this.street = this._addressObj?.street ?? '';
        this.house = this._addressObj?.house ?? '';
      },
      (error) => {
        console.error('Error loading customer', error);
      }
    );
  }
  markAsSent(orderId: string) {
    this.orderDetailService.updateOrderStatus(orderId, 'Отправлено').subscribe(
      (response) => {
        this.notificationService.showSuccess('Заказ помечен как отправленный')
        if (this.order) {
          this.order.status = 'Отправлено';
          localStorage.setItem('order', JSON.stringify(this.order));
        }
        console.log(response);
      },
      (error) => {
        const errorMessage = error?.error?.message || 'Произошла ошибка'
        this.notificationService.showError(errorMessage)
        console.error(error);
      }
    );
  }

  markAsDelivered(orderId: string): void {
    this.orderDetailService.updateOrderStatus(orderId, 'Доставлено').subscribe(() => {
    });
  }
}
