import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyIndexComponent } from './verify-index.component';

describe('VerifyIndexComponent', () => {
  let component: VerifyIndexComponent;
  let fixture: ComponentFixture<VerifyIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerifyIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
