import { NgModule } from "@angular/core";
import { HttpClientModule } from '@angular/common/http';
import { NavigationService } from "./navigation.service";

@NgModule({
    imports: [HttpClientModule],
    providers: [NavigationService]
})

export class ProductDetiailsServicesModule {
  constructor(public navService: NavigationService) { }
}
