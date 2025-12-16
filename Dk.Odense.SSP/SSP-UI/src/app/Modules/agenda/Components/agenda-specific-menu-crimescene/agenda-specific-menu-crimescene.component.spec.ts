import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaSpecificMenuCrimesceneComponent } from './agenda-specific-menu-crimescene.component';

describe('AgendaSpecificMenuCrimesceneComponent', () => {
  let component: AgendaSpecificMenuCrimesceneComponent;
  let fixture: ComponentFixture<AgendaSpecificMenuCrimesceneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaSpecificMenuCrimesceneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaSpecificMenuCrimesceneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
