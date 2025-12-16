import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Faset } from '@models/faset.model';
import { AreaRule } from '@domain-models/area-rule.model';

@Component({
  selector: 'app-automation-rules',
  templateUrl: './automation-rules.component.html',
  styleUrls: ['./automation-rules.component.scss'],
})
export class AutomationRulesComponent implements OnInit {
  @Input() areas!: Faset[];
  @Input() rules!: AreaRule[];
  @Output() pushOnCreateEvent: EventEmitter<AreaRule>;
  @Output() pushOnDeleteEvent: EventEmitter<AreaRule>;
  @Output() pushOnUpdateEvent: EventEmitter<AreaRule>;

  // model binding
  rule!: AreaRule;

  systems: string[];
  constructor() {
    this.systems = ['FASIT','DUBU','Momentum','Nexus','SBSYS'];
    this.pushOnCreateEvent = new EventEmitter<AreaRule>();
    this.pushOnDeleteEvent = new EventEmitter<AreaRule>();
    this.pushOnUpdateEvent = new EventEmitter<AreaRule>();
    this.resetModelBinding();
  }

  ngOnInit() {}

  resetModelBinding() {
    this.rule = new AreaRule();
  }

  onUpdateEvent(item: AreaRule) {
    this.pushOnUpdateEvent.emit(item);
  }

  onDeleteEvent(item: AreaRule) {
    this.pushOnDeleteEvent.emit(item);
  }

  onCreateEvent(item: AreaRule) {
    this.pushOnCreateEvent.emit(item);
    this.resetModelBinding();
  }
}
