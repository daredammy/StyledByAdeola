import { Component } from "@angular/core";
import { AuthenticationManagerService } from "src/app/services/common/authenticationManager.service";
import Swal from 'sweetalert2/dist/sweetalert2.all.js';

@Component({
  selector: "store-nav",
  templateUrl: "navBar.component.html"
})

export class NavBarComponent {
  constructor(public authService: AuthenticationManagerService) { }

  showError: boolean = false;
  smallScreen: boolean;
  email: string = "";
  password: string = "";

  login() {
    this.authService.password = this.password;
    this.authService.email = this.email;
    this.authService.login().subscribe(result => {
      if (result == false) {
        this.showError = true;
      }
      else {
        document.getElementById("closeModalButton").click();
      }
    });
  }

  logout() {
    this.authService.logout();
  }
}
