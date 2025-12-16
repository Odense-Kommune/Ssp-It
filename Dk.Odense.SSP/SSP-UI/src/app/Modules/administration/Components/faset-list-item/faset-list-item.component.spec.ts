import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FasetListItemComponent } from './faset-list-item.component';

describe('FasetListItemComponent', () => {
  let component: FasetListItemComponent;
  let fixture: ComponentFixture<FasetListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FasetListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FasetListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
