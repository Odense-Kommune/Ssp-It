import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AgendaItemService extends BaseService<any> {
  constructor(private httpClient: HttpClient) {
    super(httpClient);
    this.endpoint = 'agendaitem';
  }

  setCategorization(id: string, categorizationId: string) {
    return this.getHTTPService().put<boolean>(
      this.baseUrl +
        this.endpoint +
        '/setcategorization?id=' +
        id +
        '&categorizationId=' +
        categorizationId,
      ''
    );
  }
}
