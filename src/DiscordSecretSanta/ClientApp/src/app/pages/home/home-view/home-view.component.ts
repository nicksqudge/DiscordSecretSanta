import { Component, Input } from '@angular/core';
import { HomeResponse } from "@request/home.request";

@Component({
  selector: 'page-home-view',
  templateUrl: './home-view.component.html',
})
export class HomeViewComponent {
  public progressValue: number = 0;

  private _input: HomeResponse | undefined | null;

  @Input()
  public get input() {
    return this._input;
  }

  public set input(value: HomeResponse | undefined | null) {
    this._input = value;

    if (value == null) {
      this.progressValue = 1;
      return;
    }

    if (value.configOk == false) {
      if ((value.configDetail?.length ?? 0) > 0)
        this.progressValue = 2;
      else
        this.progressValue = 1;
      return;
    }

    if (value.admins == false) {
      this.progressValue = 3;
      return;
    }
  }

  // public showNoConfig(): boolean {
  //   return !this.input?.configOk ?? true;
  // }
  //
  // public showNotHealthy(): boolean {
  //   if (this.input === undefined)
  //     return false;
  //
  //   if (this.input.configDetail === undefined)
  //     return false;
  //
  //   return this.input.configOk === false &&
  //     this.input.configDetail.length > 0;
  // }
  //
  // public showNoAdmins(): boolean {
  //   return this.hasInput() &&
  //     this.input!.configOk === true &&
  //     this.input!.admins === false;
  // }
  //
  // public showSelectServer(): boolean {
  //   return this.hasInput() &&
  //     this.input!.configOk === true &&
  //     this.input!.admins === true &&
  //     this.input!.user !== undefined &&
  //     this.input!.user.isAdmin === true &&
  //     this.input!.activeCampaign === undefined;
  // }
  //
  // public notAdminAndNoCampaign(): boolean {
  //   return this.hasInput() &&
  //     this.input!.configOk === true &&
  //     this.input!.admins === true &&
  //     this.input!.user !== undefined &&
  //     this.input!.user.isAdmin === false;
  // }
  //
  // public showSetupCampaign(): boolean {
  //   return this.hasInput() &&
  //     this.input!.configOk === true &&
  //     this.input!.admins === true &&
  //     this.input!.user !== undefined &&
  //     this.input!.user.isAdmin === true &&
  //     this.input!.activeCampaign === undefined;
  // }
  //
  // public showCampaignHome(): boolean {
  //   return this.hasInput() &&
  //     this.input!.configOk === true &&
  //     this.input!.admins === true &&
  //     this.input!.user !== undefined &&
  //     this.input!.user.isAdmin === true &&
  //     this.input!.activeCampaign !== undefined;
  // }
  //
  // private hasInput(): boolean {
  //   return this.input !== undefined;
  // }
}
