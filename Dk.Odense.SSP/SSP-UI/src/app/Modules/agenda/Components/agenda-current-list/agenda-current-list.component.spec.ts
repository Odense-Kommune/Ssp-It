import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaCurrentListComponent } from './agenda-current-list.component';

describe('AgendaCurrentListComponent', () => {
  let component: AgendaCurrentListComponent;
  let fixture: ComponentFixture<AgendaCurrentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaCurrentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaCurrentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
