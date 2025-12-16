import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Faset } from '@models/faset.model';

@Injectable({
  providedIn: 'root',
})
export class SspAreaService extends BaseService<Faset> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'ssparea';
  }

  getFacetList() {
    return this.getHTTPService().get<Faset[]>(
      this.baseUrl + this.endpoint + '/getvalidlist'
    );
  }
}
