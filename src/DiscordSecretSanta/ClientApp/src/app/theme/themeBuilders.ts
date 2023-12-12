import { CardComponentBuilder } from "@theme/card/card.component-builder";
import { ComponentBuilder } from "@theme/component-builder";
import { CardComponentInterface } from "@theme/card/card.component-interface";
import {
  CardBodyComponentBuilder,
  CardBodyComponentInterface
} from "@theme/card/card-body/card-body.component-interface";

export interface ThemeBuilders {
  card: {
    base: ComponentBuilder<CardComponentInterface>;
    body: ComponentBuilder<CardBodyComponentInterface>;
  }
}

export class DefaultTheme implements ThemeBuilders {
  card = {
    base: new CardComponentBuilder('card')
      .hasBorder('card-border')
      .hasNoBorder('card-no-border')
      .isNormal('card-normal')
      .isCompact('card-compact'),
    body: new CardBodyComponentBuilder('card-body')
  }
}
