import { NgModule } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { RouterModule } from "@angular/router";
import { CommonModule } from '@angular/common';
import { AuthenticationManagerService } from '../services/common/authenticationManager.service';
//import { AuthenticationComponent } from "./authentication.component";
import { AuthenticationGuard } from "./authentication.guard";

@NgModule({
    imports: [RouterModule, FormsModule, CommonModule],
    //declarations: [AuthenticationComponent],
    declarations: [],
    providers: [AuthenticationManagerService, AuthenticationGuard],
    //exports: [AuthenticationComponent]
    exports: []
})
export class AuthModule { }
