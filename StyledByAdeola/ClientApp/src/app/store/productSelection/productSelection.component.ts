import { Component } from "@angular/core";

@Component({
    selector: "store-products",
    templateUrl: "productSelection.component.html"
})
export class ProductSelectionComponent {
  navactive: boolean = false;
  toggleActive() {
    this.navactive = !this.navactive;
    console.log(this.navactive);
  }
}
