import { Component, OnInit } from '@angular/core';
import { Agenda } from '@models/agenda.model';
import { PendingWorry } from '@models/pending-worry.model';
import { AgendaOverviewService } from '@services/agenda-overview.service';

@Component({
  selector: 'app-agenda-index',
  templateUrl: './agenda-index.component.html',
  styleUrls: ['./agenda-index.component.scss'],
})
export class AgendaIndexComponent implements OnInit {
  heldAgendas = new Array<Agenda>();
  currentAgendas = new Array<Agenda>();
  pendingWorries = new Array<PendingWorry>();
  busyState: boolean;

  constructor(private agendaOverviewService: AgendaOverviewService) {
    this.busyState = false;
  }

  ngOnInit() {
    this.fetchData();
  }

  onNewAgenda(agenda: Agenda) {
    this.agendaOverviewService
      .createAgenda(agenda)
      .subscribe((r) => this.currentAgendas.push(r));
  }

  onAgendaUpdate(agenda: Agenda) {
    this.agendaOverviewService.updateAgenda(agenda).subscribe((r) => {
      this.fetchData();
    });
  }

  onAgendaDateUpdate(agenda: Agenda) {
    this.agendaOverviewService.updateMeetingDate(agenda).subscribe((r) => {
      this.fetchData();
    });
  }

  onAddWorries(items: PendingWorry[]) {
    this.busyState = true;
    this.agendaOverviewService.createAgendaItems(items).subscribe((r) => {
      this.fetchData();
      this.busyState = false;
    });
  }

  fetchData() {
    this.agendaOverviewService
      .getHeldAgendas()
      .subscribe((r) => (this.heldAgendas = r));
    this.agendaOverviewService.getPendingWorries().subscribe((r) => {
      this.pendingWorries = r;
    });
    this.agendaOverviewService
      .getCurrentAgendas()
      .subscribe((r) => (this.currentAgendas = r));
  }
}
