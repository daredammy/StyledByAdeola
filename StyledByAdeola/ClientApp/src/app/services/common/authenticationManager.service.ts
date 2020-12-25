import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { map, catchError } from 'rxjs/operators';
import { Observable, of } from "rxjs";


@Injectable()
export class AuthenticationManagerService {;    

  constructor(private http: HttpClient,
              private router: Router) {}

  authenticated: boolean = false;
  email: string;
  password: string;
  callbackUrl: string;
  identityProvider: string = "Google";

  login(): Observable<boolean> {
        this.authenticated = false;
        return this.http.post<boolean>("/api/account/login", { name: this.email, password: this.password })
                        .pipe(
                              map(response => {
                                  if (response) {
                                    this.authenticated = true;
                                    this.password = null;
                                    this.router.navigateByUrl(this.callbackUrl);
                                  }
                                  return this.authenticated;
                                }
                              ),
                                catchError(e => {
                                  this.authenticated = false;
                                  return of(false);
                                }
                              ));
  }

  loginGoogle(): Observable<boolean> {
        console.log("here")
        this.authenticated = false;
        return this.http.post<boolean>("/api/account/externalidentitylogin", { name: this.email, password: this.password, identityProvider: this.identityProvider})
                        .pipe(
                              map(response => {
                                  if (response) {
                                    this.authenticated = true;
                                    this.password = null;
                                    console.log(this.callbackUrl);
                                    this.router.navigateByUrl(this.callbackUrl);
                                  }
                                  return this.authenticated;
                                }
                              ),
                          catchError(e => {
                                  console.log(e)
                                  this.authenticated = false;
                                  return of(false);
                                }
                              ));
    }

  logout() {
      this.authenticated = false;
      this.http.post("/api/account/logout", null).subscribe(response => { });
      this.router.navigateByUrl("/admin/login");
    }
}
