import { ThemeBuilders } from "@theme/themeBuilders";
import { CardComponentBuilder } from "@theme/card/card.component-builder";
import { CardBodyComponentBuilder } from "@theme/card/card-body/card-body.component-interface";

export class DaisyTheme implements ThemeBuilders {
  card = {
    base: new CardComponentBuilder('card')
      .isNormal('card-normal')
      .isCompact('card-compact')
      .hasBorder('card-bordered'),
    body: new CardBodyComponentBuilder('card-body')
  }
}
