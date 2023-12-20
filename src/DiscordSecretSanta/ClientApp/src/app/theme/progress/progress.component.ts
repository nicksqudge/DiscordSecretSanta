import { Component, Input } from '@angular/core';
import { ThemeService } from "@theme/theme.service";
import { ProgressInterface } from "@theme/progress/progress.interface";
import { Color } from "@theme/common-types";

@Component({
  selector: 'theme-progress',
  templateUrl: './progress.component.html',
})
export class ProgressComponent implements ProgressInterface {
  @Input() class: string | undefined = '';
  @Input() color: Color | undefined = 'primary';
  @Input() value: number = 0;
  @Input() max: number = 100;

  constructor(private readonly theme: ThemeService) {
    const defaults = theme.getTheme().progress.defaults;
    this.class = defaults.class;
    this.color = defaults.color;
    this.value = defaults.value;
    this.max = defaults.max;
  }

  getClass(): string {
    return this.theme.getTheme().progress.theme.buildCss(this);
  }
}
