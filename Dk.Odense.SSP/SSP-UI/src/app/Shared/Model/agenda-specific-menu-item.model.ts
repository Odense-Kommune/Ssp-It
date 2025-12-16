import { Person } from '@models/person.model';
import { WorryItem } from '@models/worry-item.model';

export class AgendaSpecificMenuItem {
  id!: string;
  person!: Person;
  worryItems!: WorryItem[];
  sortOrder!: number;
}
