import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupingsIndexComponent } from './groupings-index.component';

describe('GroupingsIndexComponent', () => {
  let component: GroupingsIndexComponent;
  let fixture: ComponentFixture<GroupingsIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupingsIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupingsIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
