import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeViewComponent } from './home-view.component';
import { By } from "@angular/platform-browser";
import { LoadingComponent } from "@theme/loading/loading.component";
import { CampaignHomeComponent } from "@app/pages/home/home-view/campaign-home/campaign-home.component";
import { NoAdminsComponent } from "@app/pages/home/home-view/no-admins/no-admins.component";
import { NoConfigComponent } from "@app/pages/home/home-view/no-config/no-config.component";
import {
  NotAdminNoCampaignComponent
} from "@app/pages/home/home-view/not-admin-no-campaign/not-admin-no-campaign.component";
import {
  NotAdminSetupInCompleteComponent
} from "@app/pages/home/home-view/not-admin-setup-in-complete/not-admin-setup-in-complete.component";
import { NotHealthyComponent } from "@app/pages/home/home-view/not-healthy/not-healthy.component";
import { SelectServerComponent } from "@app/pages/home/home-view/select-server/select-server.component";
import { SetupCampaignComponent } from "@app/pages/home/home-view/setup-campaign/setup-campaign.component";
import { ThemeModule } from "@theme/theme.module";
import { DefaultTheme } from "@theme/theme.builder";
import { CommonModule } from "@angular/common";

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
        NoAdminsComponent,
        NoConfigComponent,
        NotAdminNoCampaignComponent,
        NotAdminSetupInCompleteComponent,
        NotHealthyComponent,
        SelectServerComponent,
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
      admins: false
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
      admins: false,
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

  it('should show that the user needs to login if no admins have been setup', () => {
    component.input = {
      configOk: true,
      admins: false
    };
    fixture.detectChanges();

    expect(component.progressValue)
      .toBe(3);
    expect(fixture.debugElement.query(By.directive(NoAdminsComponent)))
      .toBeTruthy();
  });


});
