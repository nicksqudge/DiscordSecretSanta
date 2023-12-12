import { Inject, Injectable } from '@angular/core';
import { DefaultTheme, ThemeBuilders } from "@theme/themeBuilders";

@Injectable()
export class ThemeService {

  constructor(@Inject('theme') private readonly theme?: ThemeBuilders) {
  }

  getTheme(): ThemeBuilders {
    if (this.theme)
      return this.theme;

    return new DefaultTheme();
  }
}
