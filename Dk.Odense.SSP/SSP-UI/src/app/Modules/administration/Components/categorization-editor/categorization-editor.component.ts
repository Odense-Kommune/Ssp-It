import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Categorization } from '@models/categorization.model';
import { Faset } from '@models/faset.model';

@Component({
  selector: 'app-categorization-editor',
  templateUrl: './categorization-editor.component.html',
  styleUrls: ['./categorization-editor.component.scss'],
})
export class CategorizationEditorComponent implements OnInit {
  @Input() content!: Categorization[];
  @Output() pushOnCreateEvent: EventEmitter<Faset>;
  @Output() pushOnDeleteEvent: EventEmitter<Faset>;
  @Output() pushOnUpdateEvent: EventEmitter<Faset>;

  // model bindings
  newCategoryName: string;
  newExpireDays: number;
  newDeleteAfterSsp: boolean | undefined;

  constructor() {
    this.newExpireDays = 365;
    this.pushOnCreateEvent = new EventEmitter<Faset>();
    this.pushOnDeleteEvent = new EventEmitter<Faset>();
    this.pushOnUpdateEvent = new EventEmitter<Faset>();
    this.newCategoryName = '';
  }

  ngOnInit() {}

  resetModelBindings() {
    this.newCategoryName = '';
    this.newExpireDays = 365;
  }

  onDeleteEvent(item: Faset) {
    this.pushOnDeleteEvent.emit(item);
  }

  onUpdateEvent(item: Faset) {
    this.pushOnUpdateEvent.emit(item);
  }

  create() {
    if (this.newCategoryName.length === 0) {
      return;
    }
    const faset = new Faset();
    faset.daysToExpire = this.newExpireDays;
    //faset.validUntil = null;
    faset.value = this.newCategoryName;
    //faset.deleteAfterSspEnd = this.newDeleteAfterSsp;
    this.resetModelBindings();
    this.pushOnCreateEvent.emit(faset);
  }
}
