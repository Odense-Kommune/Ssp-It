import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaSpecificComponent } from './agenda-specific.component';

describe('AgendaSpecificComponent', () => {
  let component: AgendaSpecificComponent;
  let fixture: ComponentFixture<AgendaSpecificComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaSpecificComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaSpecificComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
