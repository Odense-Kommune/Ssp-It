import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AreaRule } from '@domain-models/area-rule.model';
import { Faset } from '@models/faset.model';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-rule-item',
  templateUrl: './rule-item.component.html',
  styleUrls: ['./rule-item.component.scss'],
})
export class RuleItemComponent implements OnInit {
  @Input() rule!: AreaRule;
  @Input() systems!: string[];
  @Input() areas!: Faset[];
  @Output() pushOnDeleteEvent: EventEmitter<AreaRule>;
  @Output() pushOnUpdateEvent: EventEmitter<AreaRule>;

  debounce: Subject<AreaRule>;

  constructor() {
    this.pushOnDeleteEvent = new EventEmitter<AreaRule>();
    this.pushOnUpdateEvent = new EventEmitter<AreaRule>();
    this.debounce = new Subject();
  }

  ngOnInit() {
    this.debounce.pipe(debounceTime(250)).subscribe((r) => {
      this.pushOnUpdateEvent.emit(this.rule);
    });
  }

  onDeleteEvent() {
    if (confirm('Er du sikker på at du vil slette denne regel?')) {
      this.pushOnDeleteEvent.emit(this.rule);
    }
  }

  onUpdateEvent() {
    this.debounce.next;
  }
}
