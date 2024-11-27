import { Component, ElementRef, HostListener, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../product/category/category.service';
import { AuthService } from '../Core/Services/auth.service';
import { UserDTO, UserService } from '../user/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  logoUrl = 'http://localhost:5186/images/logo/1046be01-4024-48dc-9d5e-3070a08b58ae.png';
  isUserAdmin = false;
  isUserManager = false;
  isUserLoggedIn = false;

  categories: any[] = [];
  engName: string = '';

  adminSidebarOpen = false;
  managerSidebarOpen = false;
  catalogSidebarOpen = false;

  isScreenSmall = false;
  showBurger = false;
  isSidebarToggled = false;
  canCloseSidebar = false;
  canCloseSearch = false;
  canCloseBurger = false;
  isLoading = true;

  searchTerm: string = '';
  isSearchVisible: boolean = false;

  constructor(private categoryService: CategoryService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.isUserLoggedIn = this.authService.isLoggedIn();
    if(this.isUserLoggedIn){
      this.isUserAdmin = this.authService.isAdmin();
      this.isUserManager = this.authService.isManager();
    }
    this.categoryService.getCategories().subscribe((data: any) => {
      this.categories = data.categories;
    });

    this.isScreenSmall = window.innerWidth < 610;
    this.showBurger = window.innerWidth < 800;
    this.router.events.subscribe(() => {
      const currentUrl = this.router.url;

      if (currentUrl === '/product-creater' || currentUrl === '/category-creater' || currentUrl === '/product-change') {
        this.adminSidebarOpen = window.innerWidth > 610;
      } else {
        this.adminSidebarOpen = false;
      }

      if (currentUrl === '/orders-managment') {
        this.managerSidebarOpen = window.innerWidth > 610;
      } else {
        this.managerSidebarOpen = false;
      }
    });

    this.isLoading = false;
  }

  menuOpen: boolean = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
    if (this.menuOpen) {
      this.canCloseBurger = false;
      setTimeout(() => this.canCloseBurger = true, 200); 
    }
  }

  toggleCatalogSidebar() {
    this.adminSidebarOpen = false;
    this.managerSidebarOpen = false;
    this.catalogSidebarOpen = !this.catalogSidebarOpen;

    if (this.catalogSidebarOpen) {
      this.canCloseSidebar = false;
      setTimeout(() => this.canCloseSidebar = true, 200); 
    }
  }

  toggleAdminSidebar() {
    const currentUrl = this.router.url;
    if (this.isScreenSmall || (currentUrl !== '/product-creater' && currentUrl !== '/category-creater' && currentUrl !== '/product-change')) {
      this.catalogSidebarOpen = false;
      this.adminSidebarOpen = !this.adminSidebarOpen;
      if (this.adminSidebarOpen) {
        this.canCloseSidebar = false;
        setTimeout(() => this.canCloseSidebar = true, 200); 
      }
    } else if (currentUrl === '/product-creater' || currentUrl === '/category-creater' || currentUrl === '/product-change') {
      this.adminSidebarOpen = true;
      this.canCloseSidebar = false;
      setTimeout(() => this.canCloseSidebar = true, 200); 
    }
  }

  toggleManagerSidebar() {
    const currentUrl = this.router.url;
    if (this.isScreenSmall || (currentUrl !== '/orders-managment' && currentUrl !== '/orders-managment/order-details')) {
      this.catalogSidebarOpen = false;
      this.managerSidebarOpen = !this.managerSidebarOpen;
      if (this.managerSidebarOpen) {
        this.canCloseSidebar = false;
        setTimeout(() => this.canCloseSidebar = true, 200); 
      }
    } else if (currentUrl === '/orders-managment' || currentUrl === '/orders-managment/order-details') {
      this.managerSidebarOpen = true;
      this.canCloseSidebar = false;
        setTimeout(() => this.canCloseSidebar = true, 200);
    }
  }

  toggleSearch(): void {
    if (this.searchTerm) {
      this.applyFilterSearch(this.searchTerm);
      this.searchTerm = '';
      this.isSearchVisible = !this.isSearchVisible;
      if (this.isSearchVisible) {
        this.canCloseSearch = false;
        setTimeout(() => this.canCloseSearch = true, 200); 
      }
    } else {
      this.isSearchVisible = !this.isSearchVisible;
      this.searchTerm = '';
      if (this.isSearchVisible) {
        this.canCloseSearch = false;
        setTimeout(() => this.canCloseSearch = true, 200); 
      }
    }
  }

  goToCategory(name: string): void {
    name = this.transliterate(name)
    this.router.navigate(['/categories/', name]);
  }

  applyFilterSearch(searchTerm: string): void{
    this.router.navigate(['/catalog/', searchTerm]);
  }

  logout() {
    this.authService.logout();
    this.isUserLoggedIn = false;
    this.isUserAdmin = false;
    this.isUserManager = false;

    this.router.navigate(['/']);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.isScreenSmall = window.innerWidth < 610;
    this.showBurger = window.innerWidth < 800;

    const currentUrl = this.router.url;

    if (currentUrl === '/product-creater' || currentUrl === '/category-creater' || currentUrl === '/product-change') {
      this.adminSidebarOpen = window.innerWidth > 610;
    }

    else if (currentUrl === '/orders-managment') {
      this.managerSidebarOpen = window.innerWidth > 610;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;

    const sidebar = document.querySelector('.sidebar.catalog');
    const sidebarAdmin = document.querySelector('.sidebar.admin');
    const sidebarmanager = document.querySelector('.sidebar.manager');
    const burger = document.querySelector('.burger-links');
    const searchInput = document.querySelector('.search-input-area');

    const currentUrl = this.router.url;
    if(this.canCloseBurger && burger && !burger.contains(target)){
      this.menuOpen = false;
    }
    if(this.canCloseSearch && searchInput && !searchInput.contains(target)){
      this.isSearchVisible = false;
    }
    if (this.canCloseSidebar && this.catalogSidebarOpen && sidebar && !sidebar.contains(target)) {
      this.catalogSidebarOpen = false;
    }

    else if (this.canCloseSidebar && this.adminSidebarOpen && sidebarAdmin && !sidebarAdmin.contains(target)) {
      if (this.isScreenSmall || currentUrl !== '/product-creater' && currentUrl !== '/category-creater' && currentUrl !== '/product-change') {
        this.adminSidebarOpen = false;
      }
    }

    else if (this.canCloseSidebar && this.managerSidebarOpen && sidebarmanager && !sidebarmanager.contains(target)) {
      if (this.isScreenSmall || currentUrl !== '/orders-managment') {
        this.managerSidebarOpen = false;
      }
    }
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

    for (const [key, value] of Object.entries(transliterationMap)) {
      reverseTransliterationMap[value] = key;
    }

    const map = toRussian ? reverseTransliterationMap : transliterationMap;

    return text.split('').map(char => map[char] || char).join('');
  }
}
