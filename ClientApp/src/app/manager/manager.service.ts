import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  apiUrl: string =  'http://localhost:5186';

  constructor(private http: HttpClient) {}

 
}
