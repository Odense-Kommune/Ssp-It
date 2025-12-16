import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { Agenda } from '@models/agenda.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AgendaService extends BaseService<Agenda> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'agenda';
  }
}
