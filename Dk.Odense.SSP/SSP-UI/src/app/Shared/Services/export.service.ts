import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Person } from '../model/person.model';
import { Categorization } from '../model/categorization.model';
import { PersonGrouping } from '../domain-model/person-grouping.model';
@Injectable({
  providedIn: 'root',
})
export class ExportService extends BaseService<any> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'export';
  }

  public exportAgenda(id: string): Observable<Blob> {
    return this.getHTTPService().get(
      this.baseUrl + this.endpoint + '/GetAgenda?agendaId=' + id,
      {
        responseType: 'blob' as 'blob',
      }
    );
  }

  public exportAgendaItem(id: string): Observable<Blob> {
    return this.getHTTPService().get(
      this.baseUrl + this.endpoint + '/GetAgendaItem?agendaItemId=' + id,
      {
        responseType: 'blob' as 'blob',
      }
    );
  }

  public exportCrossRef(
    id: string | null,
    selectedPersons: string[] | null,
    selectedCategorizations: string[] | null,
    groupingsType: string | null
  ): Observable<Blob> {
    return this.getHTTPService().post(
      this.baseUrl + this.endpoint + '/GetCrossRef',
      {
        id,
        selectedPersons,
        selectedCategorizations,
        groupingsType,
      },
      {
        responseType: 'blob' as 'blob',
      }
    );
  }
}
