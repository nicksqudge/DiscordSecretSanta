import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadingComponent } from './loading.component';
import { ThemeModule } from "@theme/theme.module";
import { DefaultTheme } from "@theme/theme.builder";

describe('LoadingComponent', () => {
  let component: LoadingComponent;
  let fixture: ComponentFixture<LoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ ThemeModule.forRoot(new DefaultTheme()) ],
      declarations: [ LoadingComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(LoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
