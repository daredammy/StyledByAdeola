import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductSelectionComponent } from "./store/productSelection/productSelection.component";
import { ProductDetailsComponent } from "./store/productDetails/productDetails.component";
import { CartDetailComponent } from "./store/cartDetails/cartDetail.component";
import { CheckoutShippingInfo } from "./store/checkout/checkoutDetails/checkoutShippingInfo.component";
import { CheckoutPaymentComponent } from "./store/checkout/checkoutPayment/checkoutPayment.component";
import { CheckoutSummaryComponent } from "./store/checkout/checkoutSummary/checkoutSummary.component";
import { OrderConfirmationComponent } from "./store/checkout/orderConfirmation/orderConfirmation.component";

const routes: Routes = [
  {
    path: "admin",
    loadChildren: () =>
      import("./admin/admin.module").then(module => module.AdminModule),
  },
  { path: "checkout/step1", component: CheckoutShippingInfo },
  { path: "checkout/step2", component: CheckoutPaymentComponent },
  { path: "checkout/step3", component: CheckoutSummaryComponent },
  { path: "checkout/confirmation", component: OrderConfirmationComponent },
  { path: "checkout", redirectTo: "/checkout/step1", pathMatch: "full" },
  { path: "cart", component: CartDetailComponent },
  { path: "store/p/:product", component: ProductDetailsComponent },
  { path: "store/:category/:page", component: ProductSelectionComponent },
  { path: "store/:categoryOrPage", component: ProductSelectionComponent },
  { path: "store", redirectTo: "store/", pathMatch: "full" },
  { path: "", redirectTo: "store/", pathMatch: "full" },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
