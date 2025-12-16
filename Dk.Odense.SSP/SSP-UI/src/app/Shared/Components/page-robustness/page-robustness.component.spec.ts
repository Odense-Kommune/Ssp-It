import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageRobustnessComponent } from './page-robustness.component';

describe('PageRobustnessComponent', () => {
  let component: PageRobustnessComponent;
  let fixture: ComponentFixture<PageRobustnessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageRobustnessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageRobustnessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
