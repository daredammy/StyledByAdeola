import { Injectable } from "@angular/core";
import { Payment, CartLine, OrderConfirmation, Order } from "../../models/order.model";
import { CartManagerService } from "./cartManager.service";
import { Repository } from "../../models/repository.service";
import { SessionManagerService } from "./sessionManager.service";
import { Router, NavigationStart } from "@angular/router";
import { filter } from "rxjs/operators";
import { HttpClient, HttpHeaders} from '@angular/common/http';

const ordersUrl = "/api/orders";

type OrderSession = {
    email: string,
    firstName: string,
    lastName: string,
    address1: string;
    address2: string;
    country: string;
    zip: string;
    city: string;
    state: string;
    phoneNumber: string;
    cardNumber: string,
    cardExpiry: string,
    cardSecurityCode: string
}

@Injectable()
export class OrderManagerService {

  orders: Order[] = [];
  order: Order = new Order();

  constructor(private repo: Repository,
              private sessionManager: SessionManagerService,
              private http: HttpClient,
              public cart: CartManagerService,
              router: Router) {

        router.events
        .pipe(filter(event => event instanceof NavigationStart))
        .subscribe(event => {
          if (router.url.startsWith("/checkout")
            && this.order != null) {
            sessionManager.storeSessionData<OrderSession>("checkout", {
              email: this.order.email,
              firstName: this.order.firstName,
              lastName: this.order.lastName,
              address1: this.order.address1,
              address2: this.order.address2,
              country: this.order.country,
              zip: this.order.zip,
              city: this.order.city,
              state: this.order.state,
              phoneNumber: this.order.phoneNumber,
              cardNumber: this.order.payment.cardNumber,
              cardExpiry: this.order.payment.cardExpiry,
              cardSecurityCode: this.order.payment.cardSecurityCode
              });
            }
        });

        sessionManager.getSessionData<OrderSession>("checkout").subscribe(data => {
          if (data != null) {
            this.order.email = data.email;
            this.order.firstName = data.firstName;
            this.order.lastName = data.lastName;
            this.order.address1 = data.address1;
            this.order.address2 = data.address2;
            this.order.country = data.country;
            this.order.zip = data.zip;
            this.order.city = data.city;
            this.order.state = data.state;
            this.order.phoneNumber = data.phoneNumber;
            this.order.payment.cardNumber = data.cardNumber;
            this.order.payment.cardExpiry = data.cardExpiry;
            this.order.payment.cardSecurityCode = data.cardSecurityCode
          }
        })
  }

    getOrders() {
      this.http.get<Order[]>(ordersUrl)
        .subscribe(data => this.orders = data);
    }

  createOrder(orderId: string) {
    this.order.orderId = orderId;
    this.order.products = this.products;
    var orderToSend = JSON.stringify(this.order);
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    let options = { headers: headers };
    this.http.post<OrderConfirmation>(ordersUrl, orderToSend, options)
        .subscribe(data => {
          this.order.orderConfirmation = data
          this.cart.clear();
          this.clear();
        });
    }

    shipOrder(order: Order) {
      this.http.post(`${ordersUrl}/${order.orderId}`, {})
        .subscribe(() => this.getOrders())
    }

    get products(): CartLine[] {
        return this.cart.selections
          .map(s => new CartLine(s.productId, s.name, s.price, s.imageUrl, s.selectedOptionsArray, s.quantity));
    }

    clear() {
      this.order = new Order();;
    }

    submit(orderId: string  = null) {
        this.order.submitted = true;
        this.createOrder(orderId);
    }
}
