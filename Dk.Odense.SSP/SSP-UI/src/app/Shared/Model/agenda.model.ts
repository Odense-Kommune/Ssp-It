import { AgendaItem } from '@models/agenda-item.model';

export class Agenda {
  id!: string;
  agendaName?: string;
  agendaNumber!: number;
  date!: Date;
  agendaSent!: boolean;
  meetingHeld!: boolean;
  agendaItems!: AgendaItem[];
}
