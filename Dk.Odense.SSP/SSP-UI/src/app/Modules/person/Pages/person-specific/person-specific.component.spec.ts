import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonSpecificComponent } from './person-specific.component';

describe('PersonSpecificComponent', () => {
  let component: PersonSpecificComponent;
  let fixture: ComponentFixture<PersonSpecificComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PersonSpecificComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonSpecificComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
