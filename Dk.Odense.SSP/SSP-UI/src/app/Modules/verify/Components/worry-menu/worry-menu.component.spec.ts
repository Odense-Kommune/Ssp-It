import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorryMenuComponent } from './worry-menu.component';

describe('WorryMenuComponent', () => {
  let component: WorryMenuComponent;
  let fixture: ComponentFixture<WorryMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorryMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorryMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
