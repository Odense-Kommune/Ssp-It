import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FasetEditorComponent } from './faset-editor.component';

describe('FasetEditorComponent', () => {
  let component: FasetEditorComponent;
  let fixture: ComponentFixture<FasetEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FasetEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FasetEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
