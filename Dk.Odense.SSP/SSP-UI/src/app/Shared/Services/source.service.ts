import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Faset } from '@models/faset.model';

@Injectable({
  providedIn: 'root',
})
export class SourceService extends BaseService<Faset> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'source';
  }

  public getFacetList(): Observable<Faset[]> {
    return this.getHTTPService().get<Faset[]>(
      this.baseUrl + this.endpoint + '/getvalidlist'
    );
  }
}
