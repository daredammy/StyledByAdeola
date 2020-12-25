import { Component } from "@angular/core";
import { Repository } from "../models/repository.service";
import { AuthenticationManagerService } from "../services/common/authenticationManager.service";
import { OrderManagerService } from "../services/common/orderManager.service";


@Component({
    templateUrl: "admin.component.html"
})

export class AdminComponent {
    constructor(private repo: Repository,
      public authService: AuthenticationManagerService,
      public orderSerice: OrderManagerService) {
        repo.filter.reset();
        repo.filter.related = true;
        this.repo.getProducts();
        orderSerice.getOrders();
    }
}
