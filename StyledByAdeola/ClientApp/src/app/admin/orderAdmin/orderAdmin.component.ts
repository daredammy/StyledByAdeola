import { Component } from "@angular/core";
import { Repository } from "../../models/repository.service";
import { Order } from "../../models/order.model";
import { OrderManagerService } from "../../services/common/orderManager.service";

@Component({
    templateUrl: "orderAdmin.component.html"
})
export class OrderAdminComponent {

  constructor(private repo: Repository, private order: OrderManagerService) {}

    get orders(): Order[] {
      return this.order.orders;
    }

    markShipped(order: Order) {
      this.order.shipOrder(order);
    }
}
