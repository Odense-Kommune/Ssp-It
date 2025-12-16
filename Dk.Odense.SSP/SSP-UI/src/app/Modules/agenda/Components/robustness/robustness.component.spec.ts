import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RobustnessComponent } from './robustness.component';

describe('RobustnessComponent', () => {
  let component: RobustnessComponent;
  let fixture: ComponentFixture<RobustnessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RobustnessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RobustnessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
