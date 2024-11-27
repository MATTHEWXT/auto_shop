import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
interface OrderSummary {
  totalPrice: number;
  totalCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private apiUrl = 'http://localhost:5186';

  private selectedItems: any[] = [];

  orderSummary: OrderSummary | null = null;
  constructor(private http: HttpClient, private router: Router) { }

  getCustomer(userId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/basket/checkout`, { params: { userId } });
  }

  createCustomer(userId: string, firstName: string, lastName: string, phoneNumber: string, address: string): Observable<any> {
    const customerData = {
      firstName,
      lastName,
      phoneNumber,
      address,
      userId
    };

    return this.http.post(`${this.apiUrl}/basket/checkout/create-customer`, customerData);
  }

  updateCustomer(customerId: string, firstName: string, lastName: string, phoneNumber: string, address: string, userId: string): Observable<any> {
    const customerData = {
      customerId,
      firstName,
      lastName,
      phoneNumber,
      address,
      userId
    };

    return this.http.put(`${this.apiUrl}/basket/checkout/update-customer`, customerData);
  }

  createOrder(customerId: string, itemsId: string[], totalPrice: number) {
    const orderRequest = {
      customerId,
      itemsId,
      totalPrice
    };
    return this.http.post(`${this.apiUrl}/order/create-order`, orderRequest);
  }

  setSelectedItems(items: any[]): void {
    this.selectedItems = items;
    localStorage.setItem('selectedItems', JSON.stringify(this.selectedItems ));
  }

  getSelectedItems(): any[] {
    if (this.selectedItems.length === 0) {
      const savedSelectedItems = localStorage.getItem('selectedItems');
      if (savedSelectedItems) {
        this.selectedItems = JSON.parse(savedSelectedItems);
      }
      else {
        this.router.navigate(['/basket']);
      }

      return this.selectedItems;
    }
    return this.selectedItems;
  }

}
