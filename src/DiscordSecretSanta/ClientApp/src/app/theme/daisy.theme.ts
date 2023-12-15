import { ThemeBuilders } from "@theme/themeBuilders";
import { CardComponentBuilder } from "@theme/card/card.component-builder";
import {
  CardBodyComponentBuilder,
  CardBodyComponentInterface
} from "@theme/card/card-body/card-body.component-interface";
import { CardComponentInterface } from "@theme/card/card.component-interface";

export class DaisyTheme implements ThemeBuilders {
  card = {
    base: {
      theme: new CardComponentBuilder('card')
      .isNormal('card-normal')
      .isCompact('card-compact')
      .hasBorder('card-bordered'),
      defaults: <CardComponentInterface>{
        border: true,
        mode: 'normal'
      }
    },
    body: {
      theme: new CardBodyComponentBuilder('card-body'),
      defaults: <CardBodyComponentInterface>{}
    }
  }
}
