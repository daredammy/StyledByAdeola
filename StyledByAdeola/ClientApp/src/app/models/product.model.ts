import { Rating } from "./rating.model";

export class Product {
    constructor(
      public id?: string,
      public productPage?: string, 
      public productURL?: string,
      public title?: string,
      public description?: string,
      public sku?: string,
      public optionName1?: string,
      public optionValue1?: string,
      public optionName2?: string,
      public optionValue2?: string,
      public optionName3?: string,
      public optionValue3?: string,
      public price?: number,
      public optionValuesPricePairs?: { [id: string]: string[]; },
      public optionNamesValuePairs?: { [id: string]: string[]; },
      public salesPrice?: number,
      public onSale?: string,
      public stock?: string,
      public categories?: string,
      public tags?: string[],
      public weight?: number,
      public length?: number,
      public width?: number,
      public height?: number,
      public visible?: string,
      public ratings?: Rating[],
      public imageUrls?: string,
      ) { }
}
