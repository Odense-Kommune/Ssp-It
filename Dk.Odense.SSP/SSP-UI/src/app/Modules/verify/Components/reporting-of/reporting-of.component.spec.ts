import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportingOfComponent } from './reporting-of.component';

describe('ReportingOfComponent', () => {
  let component: ReportingOfComponent;
  let fixture: ComponentFixture<ReportingOfComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportingOfComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportingOfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
