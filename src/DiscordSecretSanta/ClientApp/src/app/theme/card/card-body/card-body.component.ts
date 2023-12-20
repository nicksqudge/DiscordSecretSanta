import { Component, Input } from '@angular/core';
import { CardBodyInterface } from "@theme/card/card-body/card-body.interface";
import { ThemeService } from "@theme/theme.service";

@Component({
  selector: 'theme-card-body',
  templateUrl: './card-body.component.html',
})
export class CardBodyComponent implements CardBodyInterface {
  @Input() class: string | undefined = '';

  constructor(private readonly theme: ThemeService) {
    const defaults = theme.getTheme().card.body.defaults;
    this.class = defaults.class;
  }

  getClass(): string {
    return this.theme.getTheme().card.body.theme.buildCss(this);
  }
}
