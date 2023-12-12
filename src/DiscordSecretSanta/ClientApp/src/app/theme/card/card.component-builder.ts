import { ComponentBuilder } from "@theme/component-builder";
import { CardComponentInterface } from "@theme/card/card.component-interface";

export class CardComponentBuilder extends ComponentBuilder<CardComponentInterface> {
  public isNormal(css: string): CardComponentBuilder {
    this.add(c => c.mode == 'normal', css);
    return this;
  }

  public isCompact(css: string): CardComponentBuilder {
    this.add(c => c.mode == 'compact', css);
    return this;
  }

  public hasBorder(css: string): CardComponentBuilder {
    this.add(c => c.border, css);
    return this;
  }

  public hasNoBorder(css: string): CardComponentBuilder {
    this.add(c => c.border === false, css);
    return this;
  }
}
