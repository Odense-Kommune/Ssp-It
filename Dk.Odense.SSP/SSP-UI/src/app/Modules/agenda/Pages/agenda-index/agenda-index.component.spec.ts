import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendaIndexComponent } from './agenda-index.component';

describe('AgendaIndexComponent', () => {
  let component: AgendaIndexComponent;
  let fixture: ComponentFixture<AgendaIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
