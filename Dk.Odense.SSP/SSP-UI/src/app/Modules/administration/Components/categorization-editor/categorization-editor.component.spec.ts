import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorizationEditorComponent } from './categorization-editor.component';

describe('CategorizationEditorComponent', () => {
  let component: CategorizationEditorComponent;
  let fixture: ComponentFixture<CategorizationEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategorizationEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategorizationEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
