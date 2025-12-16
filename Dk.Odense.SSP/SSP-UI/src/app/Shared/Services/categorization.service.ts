import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Faset } from '@models/faset.model';
import { Categorization } from '@models/categorization.model';

@Injectable({
  providedIn: 'root',
})
export class CategorizationService extends BaseService<Faset> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'categorization';
  }

  getFacetList(): Observable<Categorization[]> {
    return this.getHTTPService().get<Categorization[]>(
      this.baseUrl + this.endpoint + '/getvalidlist'
    );
  }
}
