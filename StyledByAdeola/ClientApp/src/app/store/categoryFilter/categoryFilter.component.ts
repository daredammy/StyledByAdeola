import { Component } from "@angular/core";
import { NavigationService } from '../../services/productDetails/navigation.service';

@Component({
    selector: "store-categoryfilter",
    templateUrl: "categoryFilter.component.html"
})
export class CategoryFilterComponent {
  constructor(public service: NavigationService) { }

  cleanCategories(category: String) {
    var re = /\/|-/gi; 
    var cat = category.replace(re, " ").trim();
    if (cat == "byob") {
      return cat.toUpperCase()
    }
    return this.titleCase(cat);
  }

  titleCase(str) {
    var splitStr = str.toLowerCase().split(' ');
    for (var i = 0; i < splitStr.length; i++) {
      splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
    }
    return splitStr.join(' ');
  }
}
