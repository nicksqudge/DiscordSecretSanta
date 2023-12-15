import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { HomeRequest } from "@request/home.request";

@Component({
  selector: 'page-home',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {

  home$ = new HomeRequest().send(this.http);

  constructor(private readonly http: HttpClient) {
  }

}
