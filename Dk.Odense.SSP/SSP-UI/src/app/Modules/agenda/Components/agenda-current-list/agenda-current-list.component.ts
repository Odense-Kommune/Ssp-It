import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { Agenda } from '@models/agenda.model';
import { ExportService } from '@services/export.service';

@Component({
  selector: 'app-agenda-current-list',
  templateUrl: './agenda-current-list.component.html',
  styleUrls: ['./agenda-current-list.component.scss'],
})
export class AgendaCurrentListComponent implements OnInit, OnChanges {
  public myDatePickerOptions = {
    // Your options
  };
  constructor(private exportService: ExportService) {
    this.agendas = [];
    this.loaded = false;
  }
  @Input() agendas!: Agenda[];
  @Output() agendaUpdatedEvent = new EventEmitter<Agenda>();
  @Output() dateUpdatedEvent = new EventEmitter<Agenda>();
  loaded: boolean;
  isLoading = false;
  editMode: string | undefined;
  newAgendaName: string | undefined;

  ngOnInit() {}

  ngOnChanges() {
    if (!this.loaded) {
      this.loaded = !this.loaded;
    }
  }

  updateAgendaName(id: string) {
    let agenda = this.agendas.find((agenda) => agenda.id === id);
    if (agenda !== undefined) {
      agenda.agendaName = this.newAgendaName;
      this.agendaUpdatedEvent.emit(agenda);
      this.editMode = undefined;
    }
  }

  setEditMode(id: string) {
    this.editMode = id;
  }

  noAgendaName(name: string | undefined | null) {
    if (name === undefined || name === null) {
      return 'Ingen navn';
    }
    return name;
  }

  archiveAgenda(agenda: Agenda) {
    if (confirm('Vil du arkivere Dagsorden uge ' + agenda.agendaNumber)) {
      agenda.meetingHeld = true;
      this.agendaUpdatedEvent.emit(agenda);
    }
  }

  onDateUpdatedEvent(date: Date[], agenda: Agenda) {
    agenda.date = date[0];
    this.dateUpdatedEvent.emit(agenda);
  }

  getDonwloadPath(agenda: Agenda) {
    this.isLoading = true;
    return this.exportService.exportAgenda(agenda.id).subscribe((fileData) => {
      const blob = new Blob([fileData], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });

      const link = document.createElement('a');

      if (link.download !== undefined) {
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute(
          'download',
          `Dagsorden-Exel-Uge ${agenda.agendaNumber.toString()}`
        );
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }
      this.isLoading = false;
    });
  }

  getRouteUrl(agenda: Agenda) {
    return '/agendaspecific/' + agenda.id;
  }
}
