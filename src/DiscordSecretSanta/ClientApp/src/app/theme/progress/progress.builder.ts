import { ComponentBuilder } from "@theme/component-builder";
import { ProgressInterface } from "@theme/progress/progress.interface";
import { Color } from "@theme/common-types";

export class ProgressBuilder extends ComponentBuilder<ProgressInterface> {
  public onColor(color: Color, css: string): ProgressBuilder {
    this.add(c => c.color === color, css);
    return this;
  }
}
