import { CartManagerService } from '../services/common/cartManager.service';

export class ProductSelection {

  constructor(public cart: CartManagerService,
              public productId?: string,
              public name?: string,
              public price?: number,
              public imageUrl?: string,
              public selectedOptionsArray?: string[],
              private quantityValue?: number) { }

    get quantity() {
        return this.quantityValue;
    }

    set quantity(newQuantity: number) {
        this.quantityValue = newQuantity;
        this.cart.update();
    }
}
