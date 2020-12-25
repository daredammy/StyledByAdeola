import { Component, OnInit, ViewChild, ElementRef }from '@angular/core';
import { Repository } from "../../models/repository.service";
import { Product } from "../../models/product.model";
import { DomSanitizer } from '@angular/platform-browser';
import { formatCurrency } from "@angular/common";
import { CartManagerService } from '../../services/common/cartManager.service';
import { FormControl, FormGroup } from '@angular/forms';
import Swal from 'sweetalert2/dist/sweetalert2.all.js';
import { ProductService } from '../../services/productDetails/product.service';
import { Router, NavigationEnd } from '@angular/router';


@Component({
    selector: "product-details",
    templateUrl: "productDetails.component.html"
})

export class ProductDetailsComponent {

    constructor(private repo: Repository, private sanitizer: DomSanitizer, private cart: CartManagerService, private productService: ProductService, private router: Router) {
  }

    currentPicture: string;

    pictureIndex: number;

    price: number;

    optionValues:string  = "";

    quantity: number = 1;

    addToCartText: string = "Add to Cart";

    selectedOptionsArray: string[];

    get product(): Product {
      return this.productService.getProduct();
      //return this.repo.product;
    }

    get optionValuesPricePairs() {
      return this.product.optionValuesPricePairs;
     }

    get optionNamesValuePairs() {
      return this.product.optionNamesValuePairs;
    }

    get pictureUrlList(): string[] {
      if (this.product !== undefined)
      {
        var urlList = this.product.imageUrls.split(" ").slice(0, 5);
        return urlList;
      }
    };

    set currentImage(pictureUrl: string) {
      this.currentPicture = pictureUrl
    }

    get currentImage(): string {
      if (this.currentPicture == null) {
          this.pictureIndex = 0;
          return this.pictureUrlList[0];
        }
        return this.currentPicture;
    }

    sanitize(url: string) {
      return this.sanitizer.bypassSecurityTrustUrl(url);
    }

    formatUsd(num: number) {
      return formatCurrency(num, "en_us", "$", "USD");
    }

    removeSpace(str: string) {
      return str.replace(/\s+/g, '');
    }

    onOptionsSelected() {
      this.optionValues = "";
      this.selectedOptionsArray = [];
      for (let optionName in this.optionNamesValuePairs) {
        let optionValue = (<HTMLInputElement>document.getElementById((this.removeSpace(optionName)))).value;
        if (optionValue == optionName) {
          return
        }
        else {
          this.optionValues += optionValue
          this.selectedOptionsArray.push(optionValue);
        }
      }
      let valueList = this.optionValuesPricePairs[this.optionValues];
      this.price = Number(valueList[0]);
      if (Number(valueList[0]) != NaN) {
        this.product.price = Number(valueList[0]);
      }
      this.addToCartText = "Add to Cart"
    }

  addToCart(product: Product) {
      this.addToCartText= "Adding"
      let undefinedOptions = [];
      for (let optionName in this.optionNamesValuePairs) {
        let optionValue = (<HTMLInputElement>document.getElementById((this.removeSpace(optionName)))).value;
        if (optionValue == optionName) {
          undefinedOptions.push(optionValue);
        }
      }

    if (undefinedOptions.length > 0) {
        this.addToCartText = "Add to Cart"
        let undefinedOptionsString = undefinedOptions.join("; ");
        Swal.fire('', 'Please select valid option(s) for ' + undefinedOptionsString);
        return;
      }
      else {
        this.cart.addProduct(product, this.selectedOptionsArray, this.quantity);
       }
      this.addToCartText= "Added to Cart"
    }

    previousPicture() {
      if (this.pictureIndex > 0) {
        this.pictureIndex--;
        this.currentPicture = this.pictureUrlList[this.pictureIndex];
      }
    }

    nextPicture() {
      if (this.pictureIndex < this.pictureUrlList.length) {
          this.pictureIndex++;
          this.currentPicture = this.pictureUrlList[this.pictureIndex];
        }
    }

    ngOnInit() {
      this.router.events.subscribe((evt) => {
        if (!(evt instanceof NavigationEnd)) {
          return;
        }
        window.scrollTo(0, 0)
      });
    }
}
