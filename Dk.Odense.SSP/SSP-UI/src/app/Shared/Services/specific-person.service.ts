import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonMenuItem } from '@models/person-menu-item.model';

@Injectable({
  providedIn: 'root',
})
export class SpecificPersonService extends BaseService<any> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'specificperson';
  }

  getMenuItems(personId: string): Observable<PersonMenuItem[]> {
    return this.getHTTPService().get<PersonMenuItem[]>(
      this.baseUrl + this.endpoint + '/getmenuitems?personId=' + personId
    );
  }
}
