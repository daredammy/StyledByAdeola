import { Component } from "@angular/core";
import { Repository } from "../../models/repository.service";
import { Product } from "../../models/product.model";

@Component({
    selector: "admin-product-editor",
    templateUrl: "productEditor.component.html"
})
export class ProductEditorComponent {

    constructor(private repo: Repository) { }

    base64textString = [this.product.imageUrls.split(" ")[0]];

    get product(): Product {
      return this.repo.product;
    }


    onUploadChange(evt: any) {
      const file = evt.target.files[0];

      if (file) {
        const reader = new FileReader();

        reader.onload = this.handleReaderLoaded.bind(this);
        reader.readAsBinaryString(file);
      }
    }

    handleReaderLoaded(e) {
      this.base64textString[0] = ('data:image/png;base64,' + btoa(e.target.result));
      this.repo.product.imageUrls = btoa(e.target.result);
      console.log(this.repo.product.imageUrls);
    }
}
