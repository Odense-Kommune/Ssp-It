import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '@services/base.service';
import { SchoolData } from '@models/school-data.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SchoolDataService extends BaseService<SchoolData> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'schooldata';
  }

  getSchoolData(id: string): Observable<SchoolData> {
    return this.getHTTPService().get<SchoolData>(
      this.baseUrl + this.endpoint + '/getschooldata?personId=' + id
    );
  }
}
