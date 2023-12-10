import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectServerComponent } from './select-server.component';

describe('SelectServerComponent', () => {
  let component: SelectServerComponent;
  let fixture: ComponentFixture<SelectServerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ SelectServerComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SelectServerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
