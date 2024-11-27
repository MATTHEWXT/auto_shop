import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketItem } from './basket.component';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private apiUrl = 'http://localhost:5186';

  constructor(private http: HttpClient) {}

  getBasket(userId: string): Observable<any> {
    return this.http.get<BasketItem[]>(`${this.apiUrl}/basket/basketItems`, { params: { userId } });
  } 
}
