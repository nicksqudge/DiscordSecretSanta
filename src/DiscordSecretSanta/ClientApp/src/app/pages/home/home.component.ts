import { Component } from '@angular/core';
import { HomeViewComponent } from "./home-view/home-view.component";
import { HttpClient } from "@angular/common/http";
import { HomeRequest } from "@request/home.request";
import { CommonModule } from "@angular/common";

@Component({
  standalone: true,
  imports: [ HomeViewComponent, CommonModule ],
  selector: 'page-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  home$ = new HomeRequest().send(this.http);

  constructor(private readonly http: HttpClient) {
  }

}
