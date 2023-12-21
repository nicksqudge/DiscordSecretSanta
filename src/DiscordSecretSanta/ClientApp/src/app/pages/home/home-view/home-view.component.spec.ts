import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HomeViewComponent } from './home-view.component';
import { By } from "@angular/platform-browser";
import { LoadingComponent } from "@theme/loading/loading.component";
import { CampaignHomeComponent } from "@app/pages/home/home-view/campaign-home/campaign-home.component";
import { NoConfigComponent } from "@app/pages/home/home-view/no-config/no-config.component";
import {
	NotAdminNoCampaignComponent
} from "@app/pages/home/home-view/not-admin-no-campaign/not-admin-no-campaign.component";
import { NotHealthyComponent } from "@app/pages/home/home-view/not-healthy/not-healthy.component";
import { SetupCampaignComponent } from "@app/pages/home/home-view/setup-campaign/setup-campaign.component";
import { ThemeModule } from "@theme/theme.module";
import { DefaultTheme } from "@theme/theme.builder";
import { CommonModule } from "@angular/common";
import { NotSignedInComponent } from "@app/pages/home/home-view/not-signed-in/not-signed-in.component";

describe('HomeViewComponent', () => {
	let component: HomeViewComponent;
	let fixture: ComponentFixture<HomeViewComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				CommonModule,
				ThemeModule.forRoot(new DefaultTheme())
			],
			declarations: [
				CampaignHomeComponent,
				NoConfigComponent,
				NotAdminNoCampaignComponent,
				NotHealthyComponent,
				SetupCampaignComponent,
				HomeViewComponent
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(HomeViewComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	it('should show the loading spinner if the input is null', () => {
		component.input = null;

		expect(fixture.debugElement.query(By.directive(LoadingComponent)))
			.toBeTruthy();
	});

	it('should show no config when config is not okay and no details are provided', () => {
		component.input = {
			configOk: false,
		};
		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(1);
		expect(fixture.debugElement.query(By.directive(NoConfigComponent)))
			.toBeTruthy();
	});

	it('should show any health errors if there are any', () => {
		component.input = {
			configOk: false,
			configDetail: [
				{
					key: 'database',
					healthy: false,
					reason: 'Cannot connect'
				}
			]
		}
		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(2);
		expect(fixture.debugElement.query(By.directive(NotHealthyComponent)))
			.toBeTruthy();
	});

	it('should show that the user needs to login if they haven\'t already', () => {
		component.input = {
			configOk: true,
			user: undefined,
			activeCampaign: undefined
		};
		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(3);
		expect(fixture.debugElement.query(By.directive(NotSignedInComponent)))
			.toBeTruthy();
	});

	it('should show that the user is not an admin and the campaign is not setup yet', () => {
		component.input = {
			configOk: true,
			user: {
				name: 'Test',
				isAdmin: false
			}
		};

		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(4);
		expect(fixture.debugElement.query(By.directive(NotAdminNoCampaignComponent)))
			.toBeTruthy();
	});

	it('should show the campaign setup if they are an admin', () => {
		component.input = {
			configOk: true,
			user: {
				name: 'Test',
				isAdmin: true
			},
			activeCampaign: undefined
		};

		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(5);
		expect(fixture.debugElement.query(By.directive(SetupCampaignComponent)))
			.toBeTruthy();
	});

	it('should show the campaign homepage if the campaign is setup and they an admin', () => {
		component.input = {
			configOk: true,
			user: {
				name: 'Test',
				isAdmin: true
			},
			activeCampaign: {
				name: 'Test Campaign'
			}
		};

		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(6);
		expect(fixture.debugElement.query(By.directive(CampaignHomeComponent)))
			.toBeTruthy();
		expect(fixture.debugElement.query(By.directive(LoadingComponent)))
			.not.toBeTruthy();
	});

	it('should show the campaign homepage if the campaign is setup and they are not an admin', () => {
		component.input = {
			configOk: true,
			user: {
				name: 'Test',
				isAdmin: false
			},
			activeCampaign: {
				name: 'Test Campaign'
			}
		};

		fixture.detectChanges();

		expect(component.progressValue)
			.toBe(6);
		expect(fixture.debugElement.query(By.directive(CampaignHomeComponent)))
			.toBeTruthy();
		expect(fixture.debugElement.query(By.directive(LoadingComponent)))
			.not.toBeTruthy();
	});
});
