import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeViewComponent } from './home-view.component';
import { By } from "@angular/platform-browser";
import { LoadingComponent } from "../../../theme/loading/loading.component";

describe('HomeViewComponent', () => {
  let component: HomeViewComponent;
  let fixture: ComponentFixture<HomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ HomeViewComponent ]
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
    fixture.detectChanges();

    fixture.debugElement.query(By.directive(LoadingComponent));
  });
});
