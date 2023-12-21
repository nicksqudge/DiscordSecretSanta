import { NgModule } from "@angular/core";
import { HomePageComponent } from "@app/pages/home/home-page.component";
import { HomeViewComponent } from "@app/pages/home/home-view/home-view.component";
import { CampaignHomeComponent } from "@app/pages/home/home-view/campaign-home/campaign-home.component";
import { NoConfigComponent } from "@app/pages/home/home-view/no-config/no-config.component";
import {
  NotAdminNoCampaignComponent
} from "@app/pages/home/home-view/not-admin-no-campaign/not-admin-no-campaign.component";
import { NotHealthyComponent } from "@app/pages/home/home-view/not-healthy/not-healthy.component";
import { SetupCampaignComponent } from "@app/pages/home/home-view/setup-campaign/setup-campaign.component";
import { CommonModule } from "@angular/common";
import { ThemeModule } from "@theme/theme.module";
import { NotSignedInComponent } from '@app/pages/home/home-view/not-signed-in/not-signed-in.component';

@NgModule({
  declarations: [
    HomePageComponent,
    HomeViewComponent,
    CampaignHomeComponent,
    NoConfigComponent,
    NotAdminNoCampaignComponent,
    NotHealthyComponent,
    SetupCampaignComponent,
    NotSignedInComponent
  ],
  imports: [
    CommonModule,
    ThemeModule
  ],
  exports: [
    HomePageComponent
  ]
})
export class HomePageModule {
}
