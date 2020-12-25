import { Component } from "@angular/core";
import { CartManagerService } from "../../services/common/cartManager.service";

@Component({
    selector: "store-cartsummary",
    templateUrl: "cartSummary.component.html"
})
export class CartSummaryComponent {

  constructor(private cart: CartManagerService) { }

    get itemCount(): number {
        return this.cart.itemCount;
    }

    get totalPrice(): number {
        return this.cart.totalPrice;
    }
}
