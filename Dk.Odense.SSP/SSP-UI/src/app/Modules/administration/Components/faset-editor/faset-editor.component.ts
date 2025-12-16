import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs/internal/Subject';
import { Faset } from '@models/faset.model';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-faset-editor',
  templateUrl: './faset-editor.component.html',
  styleUrls: ['./faset-editor.component.scss'],
})
export class FasetEditorComponent implements OnInit {
  @Input() content!: Faset[];
  @Output() pushOnCreateEvent = new EventEmitter<string>();
  @Output() pushOnUpdateEvent = new EventEmitter<Faset>();
  @Output() pushOnDeleteEvent = new EventEmitter<Faset>();

  private updateSubject: Subject<Faset> = new Subject();

  newFasetName?: string;

  constructor() {}

  ngOnInit() {
    this.updateSubject
      .pipe(debounceTime(500))
      .subscribe((target) => this.pushOnUpdateEvent.emit(target));
  }

  createClick() {
    this.pushOnCreateEvent.emit(this.newFasetName);
    this.newFasetName = '';
  }

  deleteEvent(faset: Faset) {
    this.pushOnDeleteEvent.emit(faset);
  }

  updateEvent(faset: Faset) {
    this.pushOnUpdateEvent.next(faset);
  }
}
