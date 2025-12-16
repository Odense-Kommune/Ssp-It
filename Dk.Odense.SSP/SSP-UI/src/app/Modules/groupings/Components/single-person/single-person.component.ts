import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PersonGroups } from '@models/person-groups.model';
import { Grouping } from '@domain-models/grouping.model';
import { Classification } from '@domain-models/classification.model';
import { GuidNull } from '@shared/Constants';

@Component({
  selector: 'app-single-person',
  templateUrl: './single-person.component.html',
  styleUrls: ['./single-person.component.scss'],
})
export class SinglePersonComponent implements OnInit {
  @Input() personGroup!: PersonGroups;
  @Input() group!: Grouping;
  @Input() classifications!: Classification[];
  @Output() personRemovedEvent: EventEmitter<PersonGroups>;
  @Output() classifyPersonEvent: EventEmitter<PersonGroups>;

  selectedClassification?: string;

  constructor() {
    this.personRemovedEvent = new EventEmitter<PersonGroups>();
    this.classifyPersonEvent = new EventEmitter<PersonGroups>();
  }

  ngOnInit() {
    this.selectedClassification = this.personGroup.classification_Id;
  }

  removeFromCurrent() {
    this.personRemovedEvent.emit(this.personGroup);
  }

  setClassification(event: Event) {
    let id = (event.target as HTMLSelectElement).value;

    this.personGroup.classification_Id = id;

    this.classifyPersonEvent.emit(this.personGroup);
  }
}
