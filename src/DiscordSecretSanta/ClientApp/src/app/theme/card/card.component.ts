import { Component, Input } from '@angular/core';
import { CardComponentInterface, CardComponentMode } from "@theme/card/card.component-interface";
import { ThemeService } from "@theme/theme.service";

@Component({
  selector: 'theme-card',
  templateUrl: './card.component.html',
})
export class CardComponent implements CardComponentInterface {

  @Input() border: boolean = false;
  @Input() mode: CardComponentMode = 'normal';
  @Input() class: string = '';

  constructor(private readonly themeService: ThemeService) {
  }

  getClass(): string {
    return this.themeService.getTheme().card.base.buildCss(this);
  }
}
