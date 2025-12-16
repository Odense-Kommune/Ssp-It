import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaSpecificMenuComponent } from './agenda-specific-menu.component';

describe('AgendaSpecificMenuComponent', () => {
  let component: AgendaSpecificMenuComponent;
  let fixture: ComponentFixture<AgendaSpecificMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaSpecificMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaSpecificMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
