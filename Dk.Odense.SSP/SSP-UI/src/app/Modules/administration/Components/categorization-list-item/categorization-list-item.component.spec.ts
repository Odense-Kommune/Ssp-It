import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorizationListItemComponent } from './categorization-list-item.component';

describe('CategorizationListItemComponent', () => {
  let component: CategorizationListItemComponent;
  let fixture: ComponentFixture<CategorizationListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategorizationListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategorizationListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
