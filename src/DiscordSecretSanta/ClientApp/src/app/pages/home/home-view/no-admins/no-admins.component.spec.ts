import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoAdminsComponent } from './no-admins.component';

describe('NoAdminsComponent', () => {
  let component: NoAdminsComponent;
  let fixture: ComponentFixture<NoAdminsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoAdminsComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(NoAdminsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
