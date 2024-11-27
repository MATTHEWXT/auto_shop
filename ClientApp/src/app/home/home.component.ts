import { Component, AfterViewInit, OnInit } from '@angular/core';
import Swiper from 'swiper/bundle'; // Используйте 'swiper/bundle' для импорта
import 'swiper/swiper-bundle.css';
import { ProductService } from '../product/product.service';
import { AuthService } from '../Core/Services/auth.service';
import { NotificationService } from '../Core/notification/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, AfterViewInit {
  baseUrl: string = 'http://localhost:5186';


  slides = [
    { image: 'http://localhost:5186/images/ab58ga41twznycw7cqtyhdmc60p1l23b.webp', title: 'Slide 1' },
    { image: 'http://localhost:5186/images/ghyd6hq5iw2jme0kzp1p1fidf7az0wwo.webp', title: 'Slide 2' },
    { image: 'http://localhost:5186/images/ab58ga41twznycw7cqtyhdmc60p1l23b.webp', title: 'Slide 3' },
  ];

  products: any[] = [];

  userId: string = '';
  isAddedToBasket: { [key: string]: boolean } = {};

  isLoading = true;
  isLoggedIn = false;


  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private router: Router) { }
  swiper!: Swiper;
  swiperContainer = document.querySelector('.swiper-container') as HTMLElement;

  ngOnInit(): void {
    this.productService.getAFewProductsForHome().subscribe(data => {
      this.products = data;
      this.isLoading = false;
    });
    this.isLoggedIn = this.authService.isLoggedIn();
  }

  addProductToBasket(productId: string): void {
    if (this.isAddedToBasket[productId]) {
      this.router.navigate(['/basket']);
      return;
    }
    if (this.isLoggedIn) {
      this.userId = this.authService.getUserId() ?? '';
      this.productService.addProductToBasket(this.userId, productId);
    }
    else {
      this.notificationService.showError('Необходимо авторизоваться, чтобы добавить товар в корзину')
      return;
    }
    this.isAddedToBasket[productId] = !this.isAddedToBasket[productId];
  }

  ngAfterViewInit() {
    this.swiper = new Swiper('.swiper-container', {
      loop: true,
      speed: 500,
      effect: 'slide',
      pagination: {
        el: '.swiper-pagination',
        clickable: true,
      },
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
      },
    });
  }
}

