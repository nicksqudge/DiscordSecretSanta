import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ThemeModule } from '@theme/theme.module';
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { HomePageComponent } from "./pages/home/home-page.component";
import { DaisyTheme } from "@theme/daisy.theme";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,

    // DiscordSecretSanta
    // Pages
    HomePageComponent,

    ThemeModule.forRoot(new DaisyTheme()),
    StoreModule.forRoot({}, {})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
