export class Payment {
    cardNumber: string;
    cardExpiry: string;
    cardSecurityCode: string;
}

export class CartLine {
  constructor(private productId: string,
              private name: string,
              private price: number,
              private imageUrl: string,
              private selectedOptionsArray: string[],
              private quantity: number,
              ) { }
}

export class OrderConfirmation {
  orderId: string;
  authCode: string;
  amount: number;
}

export class Order {
  orderId: string = null;
  email: string;
  firstName: string;
  lastName: string;
  address1: string;
  address2: string;
  country: string;
  zip: string;
  city: string;
  state: string;
  phoneNumber: string;
  shipped: boolean = false;
  payment: Payment = new Payment();
  submitted: boolean = false;
  orderConfirmation: OrderConfirmation = new OrderConfirmation();
  products: CartLine[]
}
