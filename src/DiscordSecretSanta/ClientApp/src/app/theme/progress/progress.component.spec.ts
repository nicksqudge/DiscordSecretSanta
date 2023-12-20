import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressComponent } from './progress.component';
import { ThemeModule } from "@theme/theme.module";
import { DefaultTheme } from "@theme/theme.builder";

describe('ProgressComponent', () => {
  let component: ProgressComponent;
  let fixture: ComponentFixture<ProgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ ThemeModule.forRoot(new DefaultTheme()) ],
      declarations: [ ProgressComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
