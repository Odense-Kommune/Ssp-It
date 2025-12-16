import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { Observable } from 'rxjs';
import { Classification } from '../domain-model/classification.model';
import { Faset } from '../model/faset.model';

@Injectable({
  providedIn: 'root',
})
export class ClassificationService extends BaseService<Classification> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'classification';
  }

  getValidList(): Observable<Classification[]> {
    return this.getHTTPService().get<Classification[]>(
      this.baseUrl + this.endpoint + '/GetValidList'
    );
  }

  getFasetList(): Observable<Faset[]> {
    return this.getHTTPService().get<Faset[]>(
      this.baseUrl + this.endpoint + '/GetValidList'
    );
  }
}
