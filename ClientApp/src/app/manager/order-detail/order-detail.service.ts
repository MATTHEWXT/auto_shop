import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from '../../user/orders/orders.component';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderDetailService {
  apiUrl: string =  'http://localhost:5186';

  order: Order | undefined;

  constructor(private http: HttpClient, private router: Router) { }

  setOrder(order: Order): void {
    this.order = order;
    localStorage.setItem('order', JSON.stringify(order));
  }

  getOrder(): Order | undefined {
    if (this.order === undefined) {
      const savedOrder = localStorage.getItem('order');
      if (savedOrder) {
        this.order = JSON.parse(savedOrder);
      }
      else{
        this.router.navigate(['/orders-managment']);
      }
    }

    return this.order;
  }

  updateOrderStatus(orderId: string, status: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/manager/${orderId}/status`, { status });
  }
}
