import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotAdminSetupInCompleteComponent } from './not-admin-setup-in-complete.component';

describe('NotAdminSetupInCompleteComponent', () => {
  let component: NotAdminSetupInCompleteComponent;
  let fixture: ComponentFixture<NotAdminSetupInCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotAdminSetupInCompleteComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(NotAdminSetupInCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
