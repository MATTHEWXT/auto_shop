import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../Core/Services/auth.service';
import { CheckoutService } from './checkout.service';
import { NotificationService } from '../../../Core/notification/notification.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
  isLoading: boolean = true;

  customerExists: boolean = false;
  customerId: string = '';
  isEditCustomer: boolean = false;

  firstName: string = '';
  lastName: string = '';
  phoneNumber: string = '';

  address: string = '';
  _addressObj: { city: string; street: string; house: string } | null = null;

  city: string = '';
  street: string = '';
  house: string = '';

  postalCode: string = '';

  userId: string = '';

  errorMessage: string = '';
  private selectedItems: any[] = [];

  constructor(
    private authService: AuthService,
    private checkoutService: CheckoutService,
    private router: Router,
    private notificationService: NotificationService
  ) { }

  ngOnInit() {
    this.userId = this.authService.getUserId() ?? '';
    this.selectedItems = this.checkoutService.getSelectedItems();

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

        this.customerExists = true;
        this.isEditCustomer = false;

        this.isLoading = false;
      },
      (error) => {
        console.error('Error loading customer', error);
        this.isLoading = false;
        this.isEditCustomer = true;
      }
    );

  }

  editAddress() {
    this.isEditCustomer = true;
  }

  submitCustomer() {
    if (!this.firstName) {
      this.errorMessage = 'Введите имя';
      return
    }

    else if (!this.lastName) {
      this.errorMessage = 'Введите фамилию';
      return
    }

    else if (!this.phoneNumber) {
      this.errorMessage = 'Введите номер телефона';
      return
    }
    else if (!this.city) {
      this.errorMessage = 'Введите название города';
      return
    }
    else if (!this.street) {
      this.errorMessage = 'Введите название улицы';
      return
    }
    else if (!this.house) {
      this.errorMessage = 'Введите номер дома';
      return
    }

    this.firstName = this.firstName.charAt(0).toUpperCase() + this.firstName.slice(1)
    this.lastName = this.lastName.charAt(0).toUpperCase() + this.lastName.slice(1)
    this.city = this.city.charAt(0).toUpperCase() + this.city.slice(1)
    this.street = this.city.charAt(0).toUpperCase() + this.street.slice(1)

    if (!this.customerExists) {
      this.address = `{"city": "${this.city}", "street": "${this.street}", "house": "${this.house}"}`;
      this.checkoutService.createCustomer(this.userId, this.firstName, this.lastName, this.phoneNumber, this.address).subscribe(
        response => {
          console.log('Клиент создан успешно:', response);
          this.isEditCustomer = false;
          this.customerExists = true;
        },
        error => {
          const errorMessage = error?.error?.message || 'Не удалось сохранить данные';
          this.notificationService.showError(errorMessage);
          console.error('Ошибка при создании клиента:', error);
        }
      );
    }
    else {
      this.address = `{"city": "${this.city}", "street": "${this.street}", "house": "${this.house}"}`;
      this.checkoutService.updateCustomer(this.customerId, this.firstName, this.lastName, this.phoneNumber, this.address, this.userId,).subscribe(
        response => {
          this.notificationService.showSuccess('Данные сохранены');
          console.log('Клиент изменен успешно:', response);
          this.isEditCustomer = false;
        },
        error => {
          const errorMessage = error?.error?.message || 'Не удалось сохранить данные';
          this.notificationService.showError(errorMessage);
          console.error('Ошибка при изменении клиента:', error);
        }
      );
    }
  }


  createOrder() {
    const itemsId = this.selectedItems.map(item => item.id);
    this.checkoutService.createOrder(this.customerId, itemsId, this.selectedTotalPrice).subscribe(
      response => {
        this.notificationService.showSuccess('Заказ создан успешно');
        console.log('Заказ создан успешно:', response);
        this.isEditCustomer = false;
        this.router.navigate(['/basket']);

      },
      error => {
        const errorMessage = error?.error?.message || 'Не удалось создать заказ';
        this.notificationService.showError(errorMessage);
        console.error('Ошибка при создании заказа:', error);
      }
    );
  }

  get selectedTotalPrice(): number {
    return this.selectedItems
      .filter(item => item.selected)
      .reduce((total, item) => total + item.product.price, 0);
  }

  get selectedItemCount(): number {
    return this.selectedItems
      .filter(item => item.selected)
      .length;
  }

  get addressObj(): { city: string; street: string; house: string } | null {
    return typeof this._addressObj === 'object' ? this._addressObj : null;
  }
}
