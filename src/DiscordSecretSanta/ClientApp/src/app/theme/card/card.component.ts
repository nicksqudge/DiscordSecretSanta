import { Component, Input } from '@angular/core';
import { CardComponentMode, CardInterface } from "@theme/card/card.interface";
import { ThemeService } from "@theme/theme.service";

@Component({
  selector: 'theme-card',
  templateUrl: './card.component.html',
})
export class CardComponent implements CardInterface {

  @Input() border: boolean = false;
  @Input() mode: CardComponentMode = 'normal';
  @Input() class: string | undefined = '';

  constructor(private readonly themeService: ThemeService) {
    const defaults = themeService.getTheme().card.base.defaults;
    this.border = defaults.border;
    this.mode = defaults.mode;
    this.class = defaults.class;
  }

  getClass(): string {
    return this.themeService.getTheme().card.base.theme.buildCss(this);
  }
}
