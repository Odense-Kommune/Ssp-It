import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Faset } from '@models/faset.model';

@Component({
  selector: 'app-faset-list-item',
  templateUrl: './faset-list-item.component.html',
  styleUrls: ['./faset-list-item.component.scss'],
})
export class FasetListItemComponent implements OnInit {
  @Input() content!: Faset;
  @Output() PushOnUpdateEvent = new EventEmitter<Faset>();
  @Output() PushOnDeleteEvent = new EventEmitter<Faset>();

  constructor() {}

  ngOnInit() {}

  onDelete() {
    this.PushOnDeleteEvent.emit(this.content);
  }

  updateEvent() {
    this.PushOnUpdateEvent.emit(this.content);
  }
}
