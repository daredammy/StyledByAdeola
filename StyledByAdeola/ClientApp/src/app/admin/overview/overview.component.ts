import { Component } from "@angular/core";
import { Repository } from "../../models/repository.service";
import { Product } from "../../models/product.model";
import { Order } from "../../models/order.model";
import { OrderManagerService } from "../../services/common/orderManager.service";

@Component({
    templateUrl: "overview.component.html"
})
export class OverviewComponent {

  constructor(private repo: Repository, private orderSerivce: OrderManagerService) { }

    get products(): Product[] {
        return this.repo.products;
    }

    get orders(): Order[] {
      return this.orderSerivce.orders;
    }
}
