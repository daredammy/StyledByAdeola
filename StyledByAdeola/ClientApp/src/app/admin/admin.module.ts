import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { AdminComponent } from "./admin.component";
import { OverviewComponent } from "./overview/overview.component";
import { ProductAdminComponent } from "./productAdmin/productAdmin.component";
import { OrderAdminComponent } from "./orderAdmin/orderAdmin.component";
import { ProductEditorComponent } from "./productEditor/productEditor.component";
import { CommonModule } from '@angular/common';
import { AuthModule } from '../auth/auth.module';
import { AuthenticationComponent } from '../auth/authentication.component';
import { AuthenticationGuard } from '../auth/authentication.guard';
//import { CommonServicesModule } from '../services/common/commonServices.module';
//import { OrderManagerService } from '../services/common/orderManager.service';

const routes: Routes = [
    { path: "login", component: AuthenticationComponent },
    {
        path: "", component: AdminComponent,
        canActivateChild: [AuthenticationGuard],
        children: [
            { path: "products", component: ProductAdminComponent },
            { path: "orders", component: OrderAdminComponent },
            { path: "overview", component: OverviewComponent },
            { path: "", component: OverviewComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule,
    FormsModule, RouterModule.forChild(routes), CommonModule, AuthModule],
    declarations: [AdminComponent, OverviewComponent,
        ProductAdminComponent, OrderAdminComponent, ProductEditorComponent]
})
export class AdminModule { }
