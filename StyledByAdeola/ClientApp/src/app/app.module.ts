import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonServicesModule } from './services/common/commonServices.module';
import { ProductDetiailsServicesModule } from "./services/productDetails/productDetailsService.module"
import { ApplicationInsightsService } from './services/common/application-insights.service';
import { FormsModule } from '@angular/forms';
import { ModelModule } from "./models/model.module";
import { StoreModule } from "./store/store.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    CommonServicesModule,
    ProductDetiailsServicesModule,
    FormsModule,
    ModelModule,
    StoreModule
  ],
  providers: [
    ApplicationInsightsService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(appInsights: ApplicationInsightsService) { }
}
