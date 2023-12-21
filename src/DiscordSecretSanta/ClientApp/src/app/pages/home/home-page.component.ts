import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { HomeRequest, HomeResponse } from "@request/home.request";

@Component({
  selector: 'page-home',
  templateUrl: './home-page.component.html',
})
export class HomePageComponent {

  home$ = new HomeRequest().send(this.http);

  temp: HomeResponse = {
    configOk: true,
    user: undefined,
    activeCampaign: undefined
  };

  constructor(private readonly http: HttpClient) {
  }

}
