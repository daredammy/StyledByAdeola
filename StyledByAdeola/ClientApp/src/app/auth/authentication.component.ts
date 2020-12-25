import {Component } from "@angular/core";
import { AuthenticationManagerService } from "../services/common/authenticationManager.service";
import { Router } from "@angular/router";


@Component({
  templateUrl: "authentication.component.html",
  selector: "authenticate"
})
export class AuthenticationComponent {

  constructor(public authService: AuthenticationManagerService, private router: Router) {}

    showError: boolean = false;

    login() {
        this.showError = false;
        this.authService.login().subscribe(result => {
          this.showError = !result;
        });
        this.router.navigateByUrl(`/checkout/step2`);
  }

}
