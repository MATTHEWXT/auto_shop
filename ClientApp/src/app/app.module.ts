import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './Core/Services/auth.interceptor';
import { routes } from './app.routes';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ProductComponent } from './product/product.component';
import { CreaterProductComponent } from './product/creater-product/creater-product.component';
import { CategoryComponent } from './product/category/category.component';
import { CreaterCategoryComponent } from './product/category/creater-category/creater-category.component';
import { BasketComponent } from './user/basket/basket.component';
import { CheckoutComponent } from './user/basket/checkout/checkout.component';
import { OrdersComponent } from './user/orders/orders.component';
import { ManagerComponent } from './manager/manager.component';
import { OrderDetailComponent } from './manager/order-detail/order-detail.component';
import { ChangeProductComponent } from './product/change-product/change-product.component';
import { NotificationComponent } from './Core/notification/notification.component';
import { SearchProductComponent } from './product/search-product/search-product.component';
import { FooterComponent } from './footer/footer.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    ProductComponent,
    CreaterProductComponent,
    ChangeProductComponent,
    CategoryComponent,
    CreaterCategoryComponent,
    BasketComponent,
    CheckoutComponent,
    OrdersComponent,
    ManagerComponent,
    OrderDetailComponent,
    NotificationComponent,
    SearchProductComponent,
    FooterComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(routes),
    HttpClientModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'ru' },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true, 
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
