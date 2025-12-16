import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorryActionsComponent } from './worry-actions.component';

describe('WorryActionsComponent', () => {
  let component: WorryActionsComponent;
  let fixture: ComponentFixture<WorryActionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorryActionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorryActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
