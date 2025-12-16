import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '@services/base.service';
import { Faset } from '@models/faset.model';

@Injectable({
  providedIn: 'root',
})
export class PoliceWorryCategoryService extends BaseService<Faset> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'policeworrycategory';
  }

  getFacetList() {
    return this.getHTTPService().get<Faset[]>(
      this.baseUrl + this.endpoint + '/getvalidlist'
    );
  }
}
