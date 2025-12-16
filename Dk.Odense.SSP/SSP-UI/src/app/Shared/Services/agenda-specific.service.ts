import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '@services/base.service';
import { Observable } from 'rxjs';
import { AgendaSpecificMenuItem } from '@models/agenda-specific-menu-item.model';

@Injectable({
  providedIn: 'root',
})
export class AgendaSpecificService extends BaseService<object> {
  //private id: string = null;

  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'agendaspecific';
  }

  getMenuItems(id: string): Observable<AgendaSpecificMenuItem[]> {
    return this.getHTTPService().get<AgendaSpecificMenuItem[]>(
      this.baseUrl + this.endpoint + '/getmenuitems?agendaid=' + id
    );
  }
}
