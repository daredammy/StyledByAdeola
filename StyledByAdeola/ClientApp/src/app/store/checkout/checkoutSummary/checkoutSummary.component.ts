import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { OrderManagerService } from "../../../services/common/orderManager.service";

@Component({
  templateUrl: "checkoutSummary.component.html",
  selector: "store-checkoutsummary"
})
export class CheckoutSummaryComponent {

    constructor(private router: Router,
      public orderService: OrderManagerService) {
      //if (orderService.order.payment.cardNumber == null
      //  || orderService.order.payment.cardExpiry == null
      //  || orderService.order.payment.cardSecurityCode == null) {
      //      router.navigateByUrl("/checkout/step2");
      //  }
  }

    get total(): number {
        return this.orderService.cart.totalPrice + 10.75;
    }

    submitOrder() {
      this.orderService.submit();
        this.router.navigateByUrl("/checkout/confirmation");
    }
}
