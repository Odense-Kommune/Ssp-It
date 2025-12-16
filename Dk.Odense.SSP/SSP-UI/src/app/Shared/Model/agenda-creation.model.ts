import { PendingWorry } from './pending-worry.model';
import { Agenda } from './agenda.model';

export class AgendaCreation {
  pendingWorries!: PendingWorry[];
  agenda: Agenda;
  constructor() {
    this.agenda = new Agenda();
    this.agenda.meetingHeld = false;
    this.agenda.agendaSent = false;
  }
}
