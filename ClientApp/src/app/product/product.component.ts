import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProductService } from './product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../Core/Services/auth.service';
import { Subscription } from 'rxjs';
import { NotificationService } from '../Core/notification/notification.service';

@Component({
  selector: 'app-product',
  standalone: false,
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})

export class ProductComponent implements OnInit, OnDestroy {
  baseUrl = 'http://localhost:5186';
  products: any[] = [];
  userId: string = '';

  isAddedToBasket: { [key: string]: boolean } = {};

  categoryName: string | undefined;
  private routeSub: Subscription | undefined;

  loadedImagesCount = 0;
  isLoading = true;
  isLoggedIn = false;

  filteredProducts = [...this.products];
  searchTerm: string = '';
  constructor(private router: Router, private route: ActivatedRoute, private productService: ProductService, private authService: AuthService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.categoryName = params['name'];
      const name = this.route.snapshot.paramMap.get('name');
      this.productService.getProductsByCategory(name ?? '').subscribe(data => {
        this.products = data;
        this.isLoading = false;
      });
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

  filterProducts() {
    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredProducts = this.products.filter(product =>
      product.name.toLowerCase().includes(searchTermLower)
    );
  }

  ngOnDestroy(): void {
    this.routeSub?.unsubscribe();
    this.isLoading = true;

  }

  transliterate(text: string, toRussian: boolean = false): string {
    const transliterationMap: { [key: string]: string } = {
      'а': 'a', 'б': 'b', 'в': 'v', 'г': 'g', 'д': 'd', 'е': 'e', 'ё': 'yo', 'ж': 'zh',
      'з': 'z', 'и': 'i', 'й': 'yi', 'к': 'k', 'л': 'l', 'м': 'm', 'н': 'n', 'о': 'o',
      'п': 'p', 'р': 'r', 'с': 's', 'т': 't', 'у': 'u', 'ф': 'f', 'х': 'kh', 'ц': 'ts',
      'ч': 'ch', 'ш': 'sh', 'щ': 'shch', 'ъ': '', 'ы': 'y', 'ь': '', 'э': 'ie', 'ю': 'yu', 'я': 'ya',
      'А': 'A', 'Б': 'B', 'В': 'V', 'Г': 'G', 'Д': 'D', 'Е': 'E', 'Ё': 'Yo', 'Ж': 'Zh',
      'З': 'Z', 'И': 'I', 'Й': 'Yi', 'К': 'K', 'Л': 'L', 'М': 'M', 'Н': 'N', 'О': 'O',
      'П': 'P', 'Р': 'R', 'С': 'S', 'Т': 'T', 'У': 'U', 'Ф': 'F', 'Х': 'Kh', 'Ц': 'Ts',
      'Ч': 'Ch', 'Ш': 'Sh', 'Щ': 'Shch', 'Ъ': '', 'Ы': 'Y', 'Ь': '', 'Э': 'E', 'Ю': 'Yu', 'Я': 'Ya'
    };

    const reverseTransliterationMap: { [key: string]: string } = {};

    // Создаем обратный словарь
    for (const [key, value] of Object.entries(transliterationMap)) {
      reverseTransliterationMap[value] = key;
    }

    // Используем правильный словарь в зависимости от toRussian
    const map = toRussian ? reverseTransliterationMap : transliterationMap;

    // Исправляем здесь использование map
    return text.split('').map(char => map[char] || char).join('');
  }

}
