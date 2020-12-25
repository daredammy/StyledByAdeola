import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { OrderManagerService } from "../../../services/common/orderManager.service";

@Component({
    templateUrl: "orderConfirmation.component.html"
})
export class OrderConfirmationComponent {

    constructor(private router: Router,
      public orderService: OrderManagerService) {
        if (!orderService.order.submitted) {
            router.navigateByUrl("/checkout/step3");
        }
    }
}
