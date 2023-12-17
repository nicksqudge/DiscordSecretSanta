import { ThemeBuilder } from "@theme/theme.builder";
import { CardBuilder } from "@theme/card/card.builder";
import { CardBodyInterface } from "@theme/card/card-body/card-body.interface";
import { CardInterface } from "@theme/card/card.interface";
import { CardBodyBuilder } from "@theme/card/card-body/card-body.builder";
import { ComponentTheme } from "@theme/component.interface";
import { LoadingInterface } from "@theme/loading/loading.interface";
import { LoadingBuilder } from "@theme/loading/loading.builder";

export class DaisyTheme implements ThemeBuilder {
  card = {
    base: {
      theme: new CardBuilder('card bg-neutral')
        .isNormal('card-normal')
        .isCompact('card-compact')
        .hasBorder('card-bordered'),
      defaults: <CardInterface>{
        border: true,
        mode: 'normal'
      }
    },
    body: {
      theme: new CardBodyBuilder('card-body'),
      defaults: <CardBodyInterface>{}
    }
  };

  loading = <ComponentTheme<LoadingInterface>>{
    theme: new LoadingBuilder()
      .onSize('extra-small', 'loading-xs')
      .onSize('small', 'loading-sm')
      .onSize('medium', 'loading-md')
      .onSize('large', 'loading-lg'),
    defaults: {
      size: 'medium',
    }
  };
}
