import { NgModule } from "@angular/core";
import { Repository } from "./repository.service";
import { HttpClientModule } from '@angular/common/http';

@NgModule({
    imports: [HttpClientModule],
    providers: [Repository]
})
export class ModelModule {}
