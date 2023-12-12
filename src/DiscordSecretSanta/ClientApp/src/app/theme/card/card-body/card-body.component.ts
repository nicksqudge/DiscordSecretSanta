import { Component, Input } from '@angular/core';
import { CardBodyComponentInterface } from "@theme/card/card-body/card-body.component-interface";
import { ThemeService } from "@theme/theme.service";

@Component({
  selector: 'theme-card-body',
  templateUrl: './card-body.component.html',
})
export class CardBodyComponent implements CardBodyComponentInterface {
  @Input() class: string = '';

  constructor(private readonly theme: ThemeService) {
  }

  getClass(): string {
    return this.theme.getTheme().card.body.buildCss(this);
  }
}
