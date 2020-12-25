import { Product } from "./product.model";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Pagination } from "./pagination.model";
import { Filter } from "./filter.model";
import { Observable } from "rxjs";

const productsUrl = "/api/products";

type productsMetadata = {
    data: Product[],
    categories: string[];
}

@Injectable({
  providedIn: 'root',
})

export class Repository {
  product: Product;
  products: Product[];
  filter: Filter = new Filter();
  categories: string[] = [];
  paginationObject = new Pagination();

  constructor(private http: HttpClient) {
    this.filter.related = true;
  }

  getProduct(id: string) {
    this.http.get<Product>(`${productsUrl}/${id}`)
      .subscribe(p => {
        this.product = p;
      });
  }

  getProducts(): Promise<productsMetadata> {
    let url = `${productsUrl}?related=${this.filter.related}`;
    if (this.filter.category) {
      url += `&category=${this.filter.category}`;
    }
    if (this.filter.search) {
      url += `&search=${this.filter.search}`;
    }
    url += "&metadata=true";

    return this.http.get<productsMetadata>(url)
      .toPromise<productsMetadata>()
      .then(md => {
        this.products = md.data;
        this.categories = md.categories;
        return md;
      });
  }

  createProduct(prod: Product) {
    let data = {
      name: prod.title, category: prod.categories,
      description: prod.description, price: prod.price
    };

    this.http.post<string>(productsUrl, data)
      .subscribe(id => {
        prod.id = id;
        this.products.push(prod);
      });
  }

  replaceProduct(prod: Product) {
    let data = {
      name: prod.title, category: prod.categories,
      description: prod.description, price: prod.price,
      image: prod.imageUrls
    };
    this.http.put(`${productsUrl}/${prod.id}`, data)
      .subscribe(() => this.getProducts());
  }

  updateProduct(id: string, changes: Map<string, any>) {
    let patch = [];
    changes.forEach((value, key) =>
      patch.push({ op: "replace", path: key, value: value }));
    this.http.patch(`${productsUrl}/${id}`, patch)
      .subscribe(() => this.getProducts());
  }

  deleteProduct(id: string) {
    this.http.delete(`${productsUrl}/${id}`)
      .subscribe(() => this.getProducts());
  }
}
