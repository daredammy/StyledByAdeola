import { Injectable } from "@angular/core";
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot }from "@angular/router";
import { AuthenticationManagerService } from '../services/common/authenticationManager.service';

@Injectable()
export class AuthenticationGuard {

    constructor(private router: Router,
      private authService: AuthenticationManagerService) {}

    canActivateChild(route: ActivatedRouteSnapshot,
            state: RouterStateSnapshot): boolean {
        if (this.authService.authenticated) {
            return true;
        } else {
            return true; //delete for auth to work
            this.authService.callbackUrl = "/admin/"+ route.url.toString();
            this.router.navigateByUrl("/admin/login");
            return false;
        }
    }
}
