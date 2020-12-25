import { Component } from "@angular/core";
import { Repository } from "../../models/repository.service";
import { Product } from "../../models/product.model";

@Component({
    templateUrl: "productAdmin.component.html"
})
export class ProductAdminComponent {

    constructor(private repo: Repository) {}

    tableMode: boolean = true;

    get product(): Product {
        return this.repo.product;
    }

    selectProduct(id: string) {
        this.repo.getProduct(id);
    }

    saveProduct() {
        if (this.repo.product.id == null) {
            this.repo.createProduct(this.repo.product);
        } else {
            this.repo.replaceProduct(this.repo.product);
        }
        console.log(this.repo.product);
        this.clearProduct();
        this.tableMode = true;
    }

    deleteProduct(id: string) {
        this.repo.deleteProduct(id);
    }

    clearProduct() {
        this.repo.product = new Product();
        this.tableMode = true;
    }

    get products(): Product[] {
        return this.repo.products;
    }
}
