import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ProductComponent } from './product/product.component';
import { CreaterProductComponent } from './product/creater-product/creater-product.component';
import { CategoryComponent } from './product/category/category.component';
import { CreaterCategoryComponent } from './product/category/creater-category/creater-category.component';
import { AdminGuard } from './Core/guards/admin.guard';
import { BasketComponent } from './user/basket/basket.component';
import { CheckoutComponent } from './user/basket/checkout/checkout.component';
import { OrdersComponent } from './user/orders/orders.component';
import { ManagerComponent } from './manager/manager.component';
import { OrderDetailComponent } from './manager/order-detail/order-detail.component';
import { ChangeProductComponent } from './product/change-product/change-product.component';
import { ManagerGuard } from './Core/guards/manager.guard';
import { authorizedGuard } from './Core/guards/authorized';
import { SearchProductComponent } from './product/search-product/search-product.component';

export const routes: Routes = [
    {
        path: "",
        title: "Home",
        component: HomeComponent,
    },
    {
        path: "users",
        title: "Users",
        component: UserComponent,
        canActivate: [AdminGuard],
    },
    {
        path: "orders-managment",
        title: "Orders details",
        component: ManagerComponent,
        canActivate: [ManagerGuard],
    },
    {
        path: "orders-managment/order-details",
        title: "Orders managment",
        component: OrderDetailComponent,
        canActivate: [ManagerGuard],

    },
    {
        path: "user-orders",
        title: "Orders",
        component: OrdersComponent,
        canActivate: [authorizedGuard],
    },
    {
        path: "basket",
        title: "Basket",
        component: BasketComponent,
    },
    {
        path: "basket/checkout",
        title: "Basket",
        component: CheckoutComponent,
    },
    {
        path: "login",
        title: "Login",
        component: LoginComponent,
    },
    {
        path: "register",
        title: "Register",
        component: RegisterComponent,
    },
    {
        path: 'categories',
        component: CategoryComponent
    },
    {
        path: "categories/:name",
        title: "Products",
        component: ProductComponent,
    },
    {
        path: "catalog/:searchTerm",
        title: "searchTerm",
        component: SearchProductComponent,
    },
    {
        path: "product-creater",
        title: "CreateProduct",
        component: CreaterProductComponent,
        canActivate: [AdminGuard],
    },
    {
        path: "product-change",
        title: "ChangeProduct",
        component: ChangeProductComponent,
        canActivate: [AdminGuard],
    },
    {
        path: "category-creater",
        title: "CreateCategory",
        component: CreaterCategoryComponent,
        canActivate: [AdminGuard],
    },
];
