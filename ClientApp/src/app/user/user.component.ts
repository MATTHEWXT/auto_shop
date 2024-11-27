import { Component, OnInit } from '@angular/core';
import { UserService, UserDTO } from './user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit{
  users: UserDTO[] = [];

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (data) => {
        this.users = data
      },
      error: (err) => {
        console.error('Error fetching users', err);
      }
    });
  }
}
