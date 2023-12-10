import { Component, Input } from '@angular/core';
import { HomeResponse } from "@request/home.request";
import { NoConfigComponent } from "./no-config/no-config.component";
import { CommonModule } from "@angular/common";
import { NotHealthyComponent } from "@app/pages/home/home-view/not-healthy/not-healthy.component";
import { NoAdminsComponent } from "@app/pages/home/home-view/no-admins/no-admins.component";
import { SelectServerComponent } from "@app/pages/home/home-view/select-server/select-server.component";
import {
  NotAdminNoCampaignComponent
} from "@app/pages/home/home-view/not-admin-no-campaign/not-admin-no-campaign.component";
import { SetupCampaignComponent } from "@app/pages/home/home-view/setup-campaign/setup-campaign.component";
import { CampaignHomeComponent } from "@app/pages/home/home-view/campaign-home/campaign-home.component";

@Component({
  selector: 'page-home-view',
  standalone: true,
  imports: [
    CommonModule,

    NoConfigComponent,
    NotHealthyComponent,
    NoAdminsComponent,
    SelectServerComponent,
    NotAdminNoCampaignComponent,
    SetupCampaignComponent,
    CampaignHomeComponent,
  ],
  templateUrl: './home-view.component.html',
  styleUrl: './home-view.component.scss'
})
export class HomeViewComponent {
  @Input() input: HomeResponse | undefined;

  public showNoConfig(): boolean {
    return !this.input?.configOk ?? true;
  }

  public showNotHealthy(): boolean {
    if (this.input === undefined)
      return false;

    if (this.input.configDetail === undefined)
      return false;

    return this.input.configOk === false &&
      this.input.configDetail.length > 0;
  }

  public showNoAdmins(): boolean {
    return this.hasInput() &&
      this.input!.configOk === true &&
      this.input!.admins === false;
  }

  public showSelectServer(): boolean {
    return this.hasInput() &&
      this.input!.configOk === true &&
      this.input!.admins === true &&
      this.input!.user !== undefined &&
      this.input!.user.isAdmin === true &&
      this.input!.activeCampaign === undefined;
  }

  public notAdminAndNoCampaign(): boolean {
    return this.hasInput() &&
      this.input!.configOk === true &&
      this.input!.admins === true &&
      this.input!.user !== undefined &&
      this.input!.user.isAdmin === false;
  }

  public showSetupCampaign(): boolean {
    return this.hasInput() &&
      this.input!.configOk === true &&
      this.input!.admins === true &&
      this.input!.user !== undefined &&
      this.input!.user.isAdmin === true &&
      this.input!.activeCampaign === undefined;
  }

  public showCampaignHome(): boolean {
    return this.hasInput() &&
      this.input!.configOk === true &&
      this.input!.admins === true &&
      this.input!.user !== undefined &&
      this.input!.user.isAdmin === true &&
      this.input!.activeCampaign !== undefined;
  }

  private hasInput(): boolean {
    return this.input !== undefined;
  }
}
