import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeBuilder } from "@theme/theme.builder";
import { ThemeService } from "@theme/theme.service";
import { CardComponent } from "@theme/card/card.component";
import { CardBodyComponent } from "@theme/card/card-body/card-body.component";
import { LoadingComponent } from "@theme/loading/loading.component";

@NgModule({
  declarations: [
    CardComponent,
    CardBodyComponent,
    LoadingComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    CardComponent,
    CardBodyComponent,
    LoadingComponent
  ]
})
export class ThemeModule {
  static forRoot(theme: ThemeBuilder): ModuleWithProviders<ThemeModule> {
    return {
      ngModule: ThemeModule,
      providers: [
        ThemeService,
        {
          provide: 'theme',
          useValue: theme
        }
      ]
    }
  }
}
