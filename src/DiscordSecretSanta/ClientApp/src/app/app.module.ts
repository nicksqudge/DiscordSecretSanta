import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ThemeModule } from "./theme/theme.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,

    // DiscordSecretSanta Modules
    ThemeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
