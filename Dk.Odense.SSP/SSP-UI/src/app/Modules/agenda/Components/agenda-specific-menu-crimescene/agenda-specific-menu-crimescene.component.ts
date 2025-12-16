import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AgendaSpecificMenuItem } from '@models/agenda-specific-menu-item.model';
import { Person } from '@models/person.model';

@Component({
  selector: 'app-agenda-specific-menu-crimescene',
  templateUrl: './agenda-specific-menu-crimescene.component.html',
  styleUrls: ['./agenda-specific-menu-crimescene.component.scss'],
})
export class AgendaSpecificMenuCrimesceneComponent implements OnInit {
  @Input() menuItems!: AgendaSpecificMenuItem[];
  @Input() crimescene!: string;

  @Output() pushData = new EventEmitter<{
    id: string;
    person: Person;
    agendaItemId: string;
  }>();

  @Output() closeMenu = new EventEmitter();
  constructor() {}

  ngOnInit(): void {}

  pushSelectedData(id: string, person: Person, agendaItemId: string) {
    this.pushData.emit({ id, person, agendaItemId });
  }

  Close() {
    this.closeMenu.emit();
  }
}
