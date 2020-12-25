import { NgModule, NO_ERRORS_SCHEMA } from "@angular/core";
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { CartSummaryComponent } from "./cartSummary/cartSummary.component";
import { CategoryFilterComponent } from "./categoryFilter/categoryFilter.component";
import { PaginationComponent } from "./pagination/pagination.component";
import { RatingsComponent } from "./ratings/ratings.component";
import { ProductDetailsComponent } from "./productDetails/productDetails.component";
import { ProductListComponent } from "./productList/productList.component";
import { ProductSelectionComponent } from "./productSelection/productSelection.component";
import { CartDetailComponent } from "./cartDetails/cartDetail.component";
import { NavBarComponent } from "./navBar/navBar.component";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { CheckoutShippingInfo } from "./checkout/checkoutDetails/checkoutShippingInfo.component";
import { CheckoutPaymentComponent } from "./checkout/checkoutPayment/checkoutPayment.component";
import { CheckoutSummaryComponent } from "./checkout/checkoutSummary/checkoutSummary.component";
import { OrderConfirmationComponent } from "./checkout/orderConfirmation/orderConfirmation.component";
import { AuthenticationComponent } from "src/app/auth/authentication.component";
import { NgPipesModule } from 'ngx-pipes';
import { Repository } from '../models/repository.service';
import { NgxBraintreeModule } from 'ngx-braintree';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
    declarations: [CartSummaryComponent, CategoryFilterComponent,
        PaginationComponent, RatingsComponent, ProductListComponent,
        ProductSelectionComponent, CartDetailComponent, CheckoutShippingInfo,
        CheckoutPaymentComponent, CheckoutSummaryComponent, AuthenticationComponent,
        OrderConfirmationComponent, ProductDetailsComponent, NavBarComponent],
  imports: [BrowserModule, FormsModule, RouterModule, CommonModule, NgPipesModule, NgxBraintreeModule, HttpClientModule],
  exports: [ProductDetailsComponent, ProductSelectionComponent, CartDetailComponent, NavBarComponent, CheckoutShippingInfo, CheckoutSummaryComponent, AuthenticationComponent],
    schemas: [NO_ERRORS_SCHEMA],
})
export class StoreModule { }
