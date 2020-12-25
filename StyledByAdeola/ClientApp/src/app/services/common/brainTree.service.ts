import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";

const brainTreeUrl = "/api/braintree";

type Token = {
  token: string,
}

type Nonce = {
  nonce: string,
  chargeAmount: number
}


@Injectable()
export class BrainTreeService {

    tokenObject: Token;

    constructor(private http: HttpClient) {

    }

    getClientToken(): Observable<string> {
      this.http.get<Token>(`${brainTreeUrl}/getclienttoken`)
        .subscribe(t => {
          this.tokenObject = t;
        });

      return of(this.tokenObject.token);
    }

    createPurchase (nonce: string, chargeAmount: number)
    {
      let data = { nonce: nonce, chargeAmount: chargeAmount };
      return this.http.post<any>(`/api/braintree/createpurchase`, data);
    }
}
