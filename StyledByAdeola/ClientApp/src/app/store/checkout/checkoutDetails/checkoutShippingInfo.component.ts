import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { OrderManagerService } from "../../../services/common/orderManager.service";
import { AuthenticationManagerService } from "src/app/services/common/authenticationManager.service";

@Component({
  templateUrl: "checkoutShippingInfo.component.html"
})
export class CheckoutShippingInfo {

  constructor(private router: Router,
      public authService: AuthenticationManagerService,
    public orderService: OrderManagerService) {
    if (orderService.order.products != undefined && orderService.order.products.length == 0) {
            this.router.navigateByUrl("/cart");
    }            
  }

  showLoginForm: boolean = false;
  showShippingForm: boolean = false;
  showError: boolean = false;

  get showLoginForm_(): boolean {
    window.scrollTo(0, document.body.scrollHeight);
    return this.showLoginForm;
  }

  loginGoogle() {
    this.authService.callbackUrl = `/checkout/step2`;
    this.authService.loginGoogle().subscribe(result => {
      this.showError = !result;
    });
    //this.router.navigateByUrl(`/checkout/step2`);
  }
}
