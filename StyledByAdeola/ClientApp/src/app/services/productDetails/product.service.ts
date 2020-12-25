import { Product } from "../../models/product.model";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Pagination } from "../../models/pagination.model";
import { Filter } from "../../models/filter.model";
import { Observable } from "rxjs";
import { Repository } from '../../models/repository.service';


@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private repo: Repository) {
  }

  public product: Product;
  public productId: string;
  public products: Product[];

  private ran: boolean = false;

  getProducts() {
    if (this.repo.products != null && this.repo.products.length > 0) {
      this.products = this.repo.products;
      let pageIndex = (this.repo.paginationObject.currentPage - 1)
        * this.repo.paginationObject.productsPerPage;
      return this.repo.products.slice(pageIndex,
        pageIndex + this.repo.paginationObject.productsPerPage);
    }
    else {
      if (this.ran == false) {
        this.repo.getProducts();
        this.products = this.repo.products;
        this.ran = true;
      }
      if (this.repo.products != null && this.repo.products.length > 0) {
        let pageIndex = (this.repo.paginationObject.currentPage - 1)
          * this.repo.paginationObject.productsPerPage;
        return this.repo.products.slice(pageIndex,
          pageIndex + this.repo.paginationObject.productsPerPage);
      }
    }
  }

  setProductId(id: string) {
    this.productId = id;
  }

  getProduct() {
    var productId = this.productId;
    if (this.products != null && this.products.length > 0 && this.products != undefined) {
      this.product = this.products.find(function (p) { return p.id == productId; });
      return this.product;
    }
    else {
      this.getProducts();
      if (this.products != null && this.products.length > 0 && this.products != undefined) {
        this.product = this.products.find(function (p) { return p.id == productId; });
      }
      return this.product;
    }
  }
}
