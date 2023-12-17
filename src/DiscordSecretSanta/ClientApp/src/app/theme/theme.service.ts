import { Inject, Injectable } from '@angular/core';
import { DefaultTheme, ThemeBuilder } from "@theme/theme.builder";

@Injectable()
export class ThemeService {

  constructor(@Inject('theme') private readonly theme?: ThemeBuilder) {
  }

  getTheme(): ThemeBuilder {
    if (this.theme)
      return this.theme;

    return new DefaultTheme();
  }
}
