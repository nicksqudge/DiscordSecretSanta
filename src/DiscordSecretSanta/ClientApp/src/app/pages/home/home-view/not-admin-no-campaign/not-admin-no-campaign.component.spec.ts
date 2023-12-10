import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotAdminNoCampaignComponent } from './not-admin-no-campaign.component';

describe('NotAdminNoCampaignComponent', () => {
  let component: NotAdminNoCampaignComponent;
  let fixture: ComponentFixture<NotAdminNoCampaignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ NotAdminNoCampaignComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(NotAdminNoCampaignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
