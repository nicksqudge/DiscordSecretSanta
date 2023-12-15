import { CardComponentBuilder } from "@theme/card/card.component-builder";
import { ComponentBuilder } from "@theme/component-builder";
import { CardComponentInterface } from "@theme/card/card.component-interface";
import {
  CardBodyComponentBuilder,
  CardBodyComponentInterface
} from "@theme/card/card-body/card-body.component-interface";

export interface ThemeBuilders {
  card: {
    base: {
      theme: ComponentBuilder<CardComponentInterface>,
      defaults: CardComponentInterface
    }
    body: {
      theme: ComponentBuilder<CardBodyComponentInterface>,
      defaults: CardBodyComponentInterface
    }
  }
}

export class DefaultTheme implements ThemeBuilders {
  card = {
    base: {
      theme: new CardComponentBuilder('card')
        .hasBorder('card-border')
        .hasNoBorder('card-no-border')
        .isNormal('card-normal')
        .isCompact('card-compact'),
      defaults: <CardComponentInterface>{
        border: false,
        class: '',
        mode: 'normal'
      },
    },
    body: {
      theme: new CardBodyComponentBuilder('card-body'),
      defaults: <CardBodyComponentInterface>{}
    }
  }
}
