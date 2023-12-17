import { Component, Input } from '@angular/core';
import { LoadingInterface } from "@theme/loading/loading.interface";
import { Size } from "@theme/common-types";
import { ThemeService } from "@theme/theme.service";

@Component({
  selector: 'theme-loading',
  templateUrl: './loading.component.html',
})
export class LoadingComponent implements LoadingInterface {

  @Input() size: Size = 'medium';
  @Input() class: string = '';
  @Input() message: string = '';

  constructor(private readonly theme: ThemeService) {
    const defaults = theme.getTheme().loading.defaults;
    this.size = defaults.size;
    this.class = defaults.class;
    this.message = defaults.message;
  }

  getClass(): string {
    return this.theme.getTheme().loading.theme.buildCss(this);
  }
}
