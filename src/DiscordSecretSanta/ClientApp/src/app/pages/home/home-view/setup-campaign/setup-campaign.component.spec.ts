import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetupCampaignComponent } from './setup-campaign.component';

describe('SetupCampaignComponent', () => {
  let component: SetupCampaignComponent;
  let fixture: ComponentFixture<SetupCampaignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ SetupCampaignComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SetupCampaignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
