import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BackgroundJobStatusComponent } from './background-job-status.component';

describe('BackgroundJobStatusComponent', () => {
  let component: BackgroundJobStatusComponent;
  let fixture: ComponentFixture<BackgroundJobStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BackgroundJobStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BackgroundJobStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
