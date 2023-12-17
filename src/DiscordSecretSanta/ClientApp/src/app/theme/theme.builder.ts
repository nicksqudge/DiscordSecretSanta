import { CardBuilder } from "@theme/card/card.builder";
import { CardInterface } from "@theme/card/card.interface";
import { CardBodyInterface } from "@theme/card/card-body/card-body.interface";
import { CardBodyBuilder } from "@theme/card/card-body/card-body.builder";
import { ComponentTheme } from "@theme/component.interface";
import { LoadingInterface } from "@theme/loading/loading.interface";
import { LoadingBuilder } from "@theme/loading/loading.builder";

export interface ThemeBuilder {
  card: {
    base: ComponentTheme<CardInterface>,
    body: ComponentTheme<CardBodyInterface>
  };
  loading: ComponentTheme<LoadingInterface>
}

export class DefaultTheme implements ThemeBuilder {
  card = {
    base: {
      theme: new CardBuilder('card')
        .hasBorder('card-border')
        .hasNoBorder('card-no-border')
        .isNormal('card-normal')
        .isCompact('card-compact'),
      defaults: <CardInterface>{
        border: false,
        class: '',
        mode: 'normal'
      },
    },
    body: {
      theme: new CardBodyBuilder('card-body'),
      defaults: <CardBodyInterface>{}
    },
  };

  loading = <ComponentTheme<LoadingInterface>>{
    theme: new LoadingBuilder()
      .onSize('extra-small', 'loading-xs')
      .onSize('small', 'loading-sm')
      .onSize('medium', 'loading-md')
      .onSize('large', 'loading-lg')
      .onSize('extra-large', 'loading-xl'),
    defaults: {
      size: 'medium',
      class: ''
    }
  };
}
