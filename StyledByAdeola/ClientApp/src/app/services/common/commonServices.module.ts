import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { CartManagerService } from "./cartManager.service";
import { OrderManagerService } from './orderManager.service';
import { AuthenticationManagerService } from './authenticationManager.service';
import { SessionManagerService } from './sessionManager.service';
import { BrainTreeService } from './brainTree.service';


@NgModule({
    imports: [HttpClientModule],
    providers: [CartManagerService, OrderManagerService, AuthenticationManagerService, SessionManagerService, BrainTreeService]
})
export class CommonServicesModule {}
