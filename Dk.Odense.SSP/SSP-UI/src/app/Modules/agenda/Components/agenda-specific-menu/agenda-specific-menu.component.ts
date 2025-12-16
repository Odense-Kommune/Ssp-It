import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AgendaSpecificMenuItem } from '@models/agenda-specific-menu-item.model';
import { Person } from '@models/person.model';

@Component({
  selector: 'app-agenda-specific-menu',
  templateUrl: './agenda-specific-menu.component.html',
  styleUrls: ['./agenda-specific-menu.component.scss'],
})
export class AgendaSpecificMenuComponent implements OnInit {
  @Input() menuItem!: AgendaSpecificMenuItem;
  @Input() agendaNumber!: number;

  @Output() pushData = new EventEmitter<{
    id: string;
    person: Person;
    agendaItemId: string;
  }>();

  @Output() pushCrimeScene = new EventEmitter<string>();
  constructor() {}

  pushSelectedData(id: string, person: Person, agendaItemId: string) {
    this.pushData.emit({ id, person, agendaItemId });
  }

  ChoseCrimeScene(value: string) {
    this.pushCrimeScene.emit(value);
  }

  ngOnInit() {}
}
