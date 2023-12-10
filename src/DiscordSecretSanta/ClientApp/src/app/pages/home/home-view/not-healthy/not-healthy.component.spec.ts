import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotHealthyComponent } from './not-healthy.component';

describe('NotHealthyComponent', () => {
  let component: NotHealthyComponent;
  let fixture: ComponentFixture<NotHealthyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ NotHealthyComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(NotHealthyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
