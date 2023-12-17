import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardBodyComponent } from './card-body.component';
import { ThemeModule } from "@theme/theme.module";
import { DefaultTheme } from "@theme/theme.builder";

describe('CardBodyComponent', () => {
  let component: CardBodyComponent;
  let fixture: ComponentFixture<CardBodyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ ThemeModule.forRoot(new DefaultTheme()) ],
      declarations: [ CardBodyComponent ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(CardBodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
