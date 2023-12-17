import { ComponentBuilder } from "@theme/component-builder";
import { CardInterface } from "@theme/card/card.interface";

export class CardBuilder extends ComponentBuilder<CardInterface> {
  public isNormal(css: string): CardBuilder {
    this.add(c => c.mode == 'normal', css);
    return this;
  }

  public isCompact(css: string): CardBuilder {
    this.add(c => c.mode == 'compact', css);
    return this;
  }

  public hasBorder(css: string): CardBuilder {
    this.add(c => c.border, css);
    return this;
  }

  public hasNoBorder(css: string): CardBuilder {
    this.add(c => c.border === false, css);
    return this;
  }
}
