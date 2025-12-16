import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { PersonGroups } from '@models/person-groups.model';
import { Classification } from '@domain-models/classification.model';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.scss'],
})
export class PeopleComponent implements OnInit {
  @Output() personRemovedEvent: EventEmitter<PersonGroups>;
  @Output() setClassificationEvent = new EventEmitter<PersonGroups>();
  @Input() people!: PersonGroups[];
  @Input() classifications!: Classification[];

  constructor() {
    this.personRemovedEvent = new EventEmitter<PersonGroups>();
  }

  ngOnInit() {}

  onPersonRemoved(person: PersonGroups) {
    this.personRemovedEvent.emit(person);
  }

  setClassification(personGroup: PersonGroups) {
    this.setClassificationEvent.emit(personGroup);
  }
}
