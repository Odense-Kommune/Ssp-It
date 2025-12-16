import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Agenda } from '@models/agenda.model';
import { Observable } from 'rxjs';
import { PendingWorry } from '@models/pending-worry.model';

@Injectable({
  providedIn: 'root',
})
export class AgendaOverviewService extends BaseService<any> {
  constructor(private httpClient: HttpClient) {
    super(httpClient);
    this.endpoint = 'agendaoverview';
  }

  getHeldAgendas(): Observable<Agenda[]> {
    return this.getHTTPService().get<Agenda[]>(
      this.baseUrl + this.endpoint + '/getheldagendas'
    );
  }

  getCurrentAgendas(): Observable<Agenda[]> {
    return this.getHTTPService().get<Agenda[]>(
      this.baseUrl + this.endpoint + '/getcurrentagendas'
    );
  }

  getPendingWorries(): Observable<PendingWorry[]> {
    return this.getHTTPService().get<PendingWorry[]>(
      this.baseUrl + this.endpoint + '/getpendingworries'
    );
  }

  createAgenda(agenda: Agenda): Observable<Agenda> {
    return this.getHTTPService().post<Agenda>(
      this.baseUrl + this.endpoint + '/createNewAgenda',
      agenda
    );
  }

  updateMeetingDate(agenda: Agenda): Observable<Agenda> {
    return this.getHTTPService().put<Agenda>(
      this.baseUrl + this.endpoint + '/updateMeetDate',
      agenda
    );
  }

  createAgendaItems(items: PendingWorry[]): Observable<boolean> {
    return this.getHTTPService().post<boolean>(
      this.baseUrl + this.endpoint + '/createAgendaItems',
      items
    );
  }

  updateAgenda(agenda: Agenda) {
    return this.getHTTPService().put<boolean>(
      this.baseUrl + this.endpoint + '/updateagenda',
      agenda
    );
  }
}
