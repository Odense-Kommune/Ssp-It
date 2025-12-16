import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  OnChanges,
} from '@angular/core';
import { NoteAdditional } from '@domain-models/note-additional.model';
import { Note } from '@domain-models/note.model';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss'],
})
export class NotesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() notes!: Note[] | NoteAdditional[];
  @Output() SaveText = new EventEmitter<Note>();
  @Input() newNote!: Note | NoteAdditional;

  @Input() title!: string;

  disabledNew!: boolean;
  private saveTextSubject: Subject<Note | NoteAdditional> = new Subject<
    Note | NoteAdditional
  >();

  constructor() {}
  ngOnDestroy(): void {
    this.SaveText.emit(this.newNote);
  }

  ngOnChanges() {
    if (this.newNote?.id == null) {
      this.disabledNew = true;
    } else {
      this.disabledNew = false;
    }
  }

  ngOnInit() {
    this.saveTextSubject
      .pipe(debounceTime(1000))
      .subscribe((t: Note) => this.SaveText.emit(t));
  }

  onInputChange(): void {
    if (this.newNote.value == null || this.newNote.value.trim().length === 0)
      return;
    this.saveTextSubject.next(this.newNote);
  }
}
