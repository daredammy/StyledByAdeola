import { Component } from "@angular/core";
import { CartManagerService } from "../../services/common/cartManager.service";

@Component({
    templateUrl: "cartDetail.component.html"
})
export class CartDetailComponent {
  constructor(public cart: CartManagerService) { }
}
