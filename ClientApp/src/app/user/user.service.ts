import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../Core/Services/auth.service';

export interface UserDTO {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5186';

  userId: string = '' ;
  constructor(private http: HttpClient, private authService: AuthService) { }

  getUsers(): Observable<UserDTO[]> {
    return this.http.get<UserDTO[]>(`${this.apiUrl}/user`);
  }

  getUser(): Observable<UserDTO>{
    this.userId = this.authService.getUserId() || '';
    return this.http.get<UserDTO>(`${this.apiUrl}/user/${this.userId}`);
  }
}
