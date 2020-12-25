import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { OrderManagerService } from "../../../services/common/orderManager.service";
import { CartManagerService } from "../../../services/common/cartManager.service";
import { NgxBraintreeModule } from 'ngx-braintree';
import { HttpClientModule } from '@angular/common/http';
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import Swal from 'sweetalert2/dist/sweetalert2.js';
import * as sezzle from 'src/assets/scripts/sezzleCheckout.js';


type Token = {
  token: string,
}

type Nonce = {
  nonce: string,
  chargeAmount: number
}


@Component({
    templateUrl: "checkoutPayment.component.html"
})
export class CheckoutPaymentComponent {
  tokenObject: Token;

  constructor(private router: Router,
              public orderService: OrderManagerService,
              public cartService: CartManagerService,
              private http: HttpClient)
  {
    if (orderService.order.firstName == null || orderService.order.zip == null) {
            router.navigateByUrl("/checkout/step1");
    }

    console.log(cartService.selections.length);

    //if (cartService.selections.length == 0) {
    //  router.navigateByUrl("/cart");
    //}
  }

  chargeAmount: number = this.orderService.cart.totalPrice;

  onPaymentStatus(event) {
    console.log(event.target);
    console.log(event.target.orderId);
    if (event.errors == null) {
      var orderId = event.target.value.orderId
      this.orderService.submit(orderId);
      Swal.fire('', 'Order Complete');
      //this.router.navigateByUrl("/checkout/step3");
    }
  }
}
