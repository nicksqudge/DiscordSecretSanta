import { Component } from '@angular/core';
import { HomeViewComponent } from "./home-view/home-view.component";
import { HttpClient } from "@angular/common/http";
import { HomeRequest } from "@request/home.request";
import { CommonModule } from "@angular/common";
import { ThemeModule } from "@theme/theme.module";

@Component({
  standalone: true,
  imports: [ HomeViewComponent, CommonModule, ThemeModule ],
  selector: 'page-home',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {

  home$ = new HomeRequest().send(this.http);

  constructor(private readonly http: HttpClient) {
  }

}
