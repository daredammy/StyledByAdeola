import { Injectable } from "@angular/core";
import { Product } from "../../models/product.model";
import { CartLine } from "../../models/order.model";
import { Repository } from '../../models/repository.service';
import { ProductSelection } from '../../models/cart.model';
import { SessionManagerService } from './sessionManager.service';


@Injectable()
export class CartManagerService {
    selections: ProductSelection[] = [];
    itemCount: number = 0;
    totalPrice: number = 0;

    constructor(private repo: Repository, private sessionManager: SessionManagerService) {
      sessionManager.getSessionData<ProductSelection[]>("cart").subscribe(cartData => {
          if (cartData != null) {
              cartData.forEach(item => this.selections.push(item));
              this.update(false);
          }
      });
    }

    addProduct(product: Product, selectedOptionsArray: string[], quantity:number = 1) {
          let selection = this.selections
              .find(ps => ps.productId == product.id);
          if (selection) {
              selection.quantity++;
          } else {
              this.selections.push(new ProductSelection(this,
                  product.id, product.title,
                  product.price, product.imageUrls.split(" ")[0],
                  selectedOptionsArray,
                  quantity));
          }
          this.update();
      }

    updateQuantity(productId: string, quantity: number) {
          if (quantity > 0) {
              let selection = this.selections.find(ps => ps.productId == productId);
              if (selection) {
                  selection.quantity = quantity;
              }
          } else {
              let index = this.selections.findIndex(ps => ps.productId == productId);
              if (index != -1) {
                  this.selections.splice(index, 1);
              }
              this.update();
          }
    }

    updateTotalPrice() {
      this.totalPrice = this.selections.map(ps => ps.price * ps.quantity)
        .reduce((prev, curr) => prev + curr, 0);
    }

    clear() {
        this.selections = [];
        this.update();
    }

    update(storeData: boolean = true) {
        this.itemCount = this.selections.map(ps => ps.quantity)
            .reduce((prev, curr) => prev + curr, 0);
        this.totalPrice = this.selections.map(ps => ps.price * ps.quantity)
            .reduce((prev, curr) => prev + curr, 0);
      if (storeData) {
          this.sessionManager.storeSessionData("cart", this.selections
            .map(s => new CartLine(s.productId, s.name, s.price, s.imageUrl, s.selectedOptionsArray, s.quantity)));
        }
    }
}
