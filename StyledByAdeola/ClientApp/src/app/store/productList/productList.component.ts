import { Component } from "@angular/core";
import { formatCurrency } from "@angular/common";
import { Repository } from "../../models/repository.service";
import { Product } from "../../models/product.model";
import { CartManagerService } from '../../services/common/cartManager.service';
import { DomSanitizer } from '@angular/platform-browser';
import { NavigationService } from '../../services/productDetails/navigation.service';
import { ProductService } from '../../services/productDetails/product.service';

@Component({
    selector: "store-product-list",
    templateUrl: "productList.component.html"
    //providers: [Repository]
})
export class ProductListComponent {

  constructor(private repo: Repository, private productService:ProductService, private cart: CartManagerService, private sanitizer: DomSanitizer, public navService: NavigationService) { }

  get products(): Product[] {
    return this.productService.getProducts();
    //if (this.repo.products != null && this.repo.products.length > 0) {
    //      let pageIndex = (this.repo.paginationObject.currentPage - 1)
    //          * this.repo.paginationObject.productsPerPage;
    //      return this.repo.products.slice(pageIndex,
    //          pageIndex + this.repo.paginationObject.productsPerPage);
    //  }
    }

  sanitize(product: Product, index: number) {
    var urlList = product.imageUrls.split(" ");
    var url = urlList[0];
    if (index >= 0 && index < urlList.length) {
      url = urlList[index.toString()];
    }
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }

  formatUsd(num: number) {
    return "from " + formatCurrency(num, "en_us","$","USD");
  }
}
