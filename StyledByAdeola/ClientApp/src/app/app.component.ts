import { Component, OnInit } from '@angular/core';
import { ApplicationInsightsService } from './services/common/application-insights.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'insights';

  constructor(private router: Router) {
     new ApplicationInsightsService(router);
  }
}
