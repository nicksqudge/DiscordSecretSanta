import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeBuilders } from "@theme/themeBuilders";
import { ThemeService } from "@theme/theme.service";
import { CardComponent } from "@theme/card/card.component";
import { CardBodyComponent } from "@theme/card/card-body/card-body.component";

@NgModule({
  declarations: [
    CardComponent,
    CardBodyComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    CardComponent,
    CardBodyComponent
  ]
})
export class ThemeModule {
  static forRoot(theme: ThemeBuilders): ModuleWithProviders<ThemeModule> {
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
