import { ComponentInterface } from "@theme/component.interface";

export abstract class ComponentBuilder<TComponent extends ComponentInterface> {
  private actions: ComponentBuilderAction<TComponent>[] = [];
  private readonly startingCss: string = '';

  constructor(startingCss: string = '') {
    this.startingCss = startingCss;
  }

  public buildCss(component: TComponent): string {
    let result = this.startingCss;
    for (const action of this.actions) {
      if (action.is(component))
        result += " " + action.cssToApply;
    }

    if (component.class)
      result += " " + component.class;

    return result;
  }

  protected add(is: ComponentCheck<TComponent>, cssToApply: string): void {
    this.actions.push({
      is,
      cssToApply
    });
  }
}

export type ComponentCheck<TComponent> = (component: TComponent) => boolean;

export type ComponentBuilderAction<TComponent> = {
  is: ComponentCheck<TComponent>;
  cssToApply: string;
}
