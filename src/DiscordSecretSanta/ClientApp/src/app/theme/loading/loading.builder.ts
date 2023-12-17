import { ComponentBuilder } from "@theme/component-builder";
import { LoadingInterface } from "@theme/loading/loading.interface";
import { Size } from "@theme/common-types";

export class LoadingBuilder extends ComponentBuilder<LoadingInterface> {
  public onSize(size: Size, css: string): LoadingBuilder {
    this.add(c => c.size == size, css);
    return this;
  }
}


