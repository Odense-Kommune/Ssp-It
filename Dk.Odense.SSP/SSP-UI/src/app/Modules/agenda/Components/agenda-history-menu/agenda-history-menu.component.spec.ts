import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaHistoryMenuComponent } from './agenda-history-menu.component';

describe('AgendaHistoryMenuComponent', () => {
  let component: AgendaHistoryMenuComponent;
  let fixture: ComponentFixture<AgendaHistoryMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaHistoryMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaHistoryMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
