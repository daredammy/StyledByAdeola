import { Injectable } from "@angular/core";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { Repository } from '../../models/repository.service';
import { Product } from "../../models/product.model";
import { filter } from "rxjs/operators";
import { ProductService } from '../../services/productDetails/product.service';


@Injectable()
export class NavigationService {
    private product: Product;
    constructor(private repository: Repository, private router: Router, 
                private active: ActivatedRoute, private productService: ProductService,) {
        router.events
            .pipe(filter(event => event instanceof NavigationEnd))
            .subscribe(ev => this.handleNavigationChange());
    }

    private handleNavigationChange() {
      let active = this.active.firstChild.snapshot;
      if (active.url.length > 0 && active.url[0].path === "store" && active.url[1].path === "p")
      {
        this.handleProductNavigationChange();
      }
      else if (active.url.length > 0 && active.url[0].path === "store")
      {
        this.repository.filter.search = "";
        if (active.params["categoryOrPage"] !== undefined)
        {
          // if url defines a category, check if it's a page num or category
          let value = Number.parseInt(active.params["categoryOrPage"]);
          if (!Number.isNaN(value)) {
            this.repository.filter.category = "";
            this.repository.paginationObject.currentPage = value;
          }
          else
          {
            this.repository.filter.category
              = active.params["categoryOrPage"];
            this.repository.paginationObject.currentPage = 1;
          }
        }
        else
        {
          // if url doesn't define a category
          let category = active.params["category"];
          this.repository.filter.category = category || "";
          this.repository.paginationObject.currentPage
            = Number.parseInt(active.params["page"]) || 1
        }
        this.repository.paginationObject.productsPerPage = 12;
        //this.repository.getProducts();
      }
    }

    private handleProductNavigationChange()
    {
        let active = this.active.firstChild.snapshot;
        if (active.params["product"] !== undefined) {
          this.productService.setProductId(active.params["product"]);
          this.repository.paginationObject.productsPerPage = 6;
          //this.repository.getProducts();
        }
      }

    get categories(): string[] {
        return this.repository.categories;
    }

    get currentCategory(): string {
        return this.repository.filter.category;
    }

    set currentCategory(newCategory: string) {
        newCategory = newCategory.replace("/", "");
        //set the category and try to naviag to the url- url should show in browser
        this.router.navigateByUrl(`/store/${(newCategory || "").toLowerCase()}`);
    }

    set currentProduct(product: Product) {
        this.product = product;
        this.router.navigateByUrl(`/store/p/${(product.id.trim() || "").toLowerCase()}`);
    }

    get currentPage(): number {
        return this.repository.paginationObject.currentPage;
    }

    set currentPage(newPage: number) {
        if (this.currentCategory === "") {
            this.router.navigateByUrl(`/store/${newPage}`);
        } else {
            this.router.navigateByUrl(`/store/${this.currentCategory}/${newPage}`);
        }
    }

    get productsPerPage(): number {
        return this.repository.paginationObject.productsPerPage;
    }

    get productCount(): number {
        return (this.repository.products || []).length;
    }
}
