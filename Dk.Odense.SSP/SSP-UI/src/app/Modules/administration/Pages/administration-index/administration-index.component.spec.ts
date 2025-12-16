import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationIndexComponent } from './administration-index.component';

describe('AdministrationIndexComponent', () => {
  let component: AdministrationIndexComponent;
  let fixture: ComponentFixture<AdministrationIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
