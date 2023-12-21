import { Component, Input } from '@angular/core';
import { HomeResponse } from "@request/home.request";

@Component({
	selector: 'page-home-view',
	templateUrl: './home-view.component.html',
})
export class HomeViewComponent {
	private static NoConfig: number = 1;
	private static NotHealthy: number = 2;
	private static NotSignedIn: number = 3;
	private static NotAdminAndNoCampaign: number = 4;
	private static SetupCampaign: number = 5;
	private static CampaignHome: number = 6;

	public progressValue: number = 0;

	private _input: HomeResponse | undefined | null;

	@Input()
	public get input() {
		return this._input;
	}

	public set input(value: HomeResponse | undefined | null) {
		this._input = value;
		this.progressValue = this.getProgressValue(value);
	}

	private getProgressValue(value: HomeResponse | undefined | null): number {
		// The config is missing
		if (value == null) {
			return HomeViewComponent.NoConfig;
		}

		// The config is not okay
		if (value.configOk === false) {
			if ((value.configDetail?.length ?? 0) > 0)
				return HomeViewComponent.NotHealthy;
			else
				return HomeViewComponent.NoConfig;
		}

		if (value.user == null) {
			return HomeViewComponent.NotSignedIn;
		} else {
			if (value.activeCampaign == null) {
				return value.user.isAdmin ?
					HomeViewComponent.SetupCampaign :
					HomeViewComponent.NotAdminAndNoCampaign;
			} else {
				return HomeViewComponent.CampaignHome;
			}
		}
	}
}
