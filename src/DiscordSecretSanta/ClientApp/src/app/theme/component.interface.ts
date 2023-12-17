import { ComponentBuilder } from "@theme/component-builder";

export interface ComponentInterface {
  class?: string;
}

export interface ComponentTheme<T extends ComponentInterface> {
  theme: ComponentBuilder<T>;
  defaults: T;
}
