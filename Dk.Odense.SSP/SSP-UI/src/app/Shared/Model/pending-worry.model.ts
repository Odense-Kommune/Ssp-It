import { Person } from '@models/person.model';
import { Worry } from '@domain-models/worry.model';

export class PendingWorry {
  person!: Person;
  worries!: Worry[];
  checked = true;
  agendaId!: string;
}
