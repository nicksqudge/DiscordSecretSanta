import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoConfigComponent } from './no-config.component';

describe('NoConfigComponent', () => {
  let component: NoConfigComponent;
  let fixture: ComponentFixture<NoConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoConfigComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(NoConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
