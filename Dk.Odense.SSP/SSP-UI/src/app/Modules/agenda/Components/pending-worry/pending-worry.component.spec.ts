import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingWorryComponent } from './pending-worry.component';

describe('PendingWorryComponent', () => {
  let component: PendingWorryComponent;
  let fixture: ComponentFixture<PendingWorryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingWorryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingWorryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
