import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Faset } from '@models/faset.model';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-categorization-list-item',
  templateUrl: './categorization-list-item.component.html',
  styleUrls: ['./categorization-list-item.component.scss'],
})
export class CategorizationListItemComponent implements OnInit {
  @Input() content!: Faset;
  @Output() deleteEvent: EventEmitter<Faset>;
  @Output() updateEvent: EventEmitter<Faset>;
  debounce: Subject<string> = new Subject();

  constructor() {
    this.deleteEvent = new EventEmitter<Faset>();
    this.updateEvent = new EventEmitter<Faset>();
  }

  ngOnInit() {
    this.debounce.pipe(debounceTime(250)).subscribe((r) => {
      this.updateEvent.emit(this.content);
    });
  }

  delete(item: Faset) {
    if (confirm("Vil du slette '" + this.content.value + "'?")) {
      this.deleteEvent.emit(item);
    }
  }

  update() {
    this.debounce.next;
  }
}
