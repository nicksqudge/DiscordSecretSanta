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

    expect(fixture.debugElement.query(By.directive(NoConfigComponent)))
      .toBeTruthy();
  });


});
