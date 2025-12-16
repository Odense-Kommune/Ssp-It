import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '@models/person.model';
import { BaseService } from '@services/base.service';
import { Observable } from 'rxjs';
import { SocialSecData } from '@models/SocialSecData.model';

@Injectable({
  providedIn: 'root',
})
export class PersonService extends BaseService<Person> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'person';
  }

  getSocialSecData(socialSecNum: string): Observable<SocialSecData> {
    return this.getHTTPService().get<SocialSecData>(
      this.baseUrl +
        this.endpoint +
        '/getsocialsecdata?socialsecnum=' +
        socialSecNum
    );
  }

  searchByNameOrGroup(searchTerm: string): Observable<Person[]> {
    return this.getHTTPService().get<Person[]>(
      this.baseUrl + this.endpoint + '/searchGroupAndName?term=' + searchTerm
    );
  }

  searchByCpr(searchTerm: string): Observable<Person[]> {
    return this.getHTTPService().get<Person[]>(
      this.baseUrl + this.endpoint + '/searchCpr?term=' + searchTerm
    );
  }

  setSspArea(id: string, sspAreaId: string): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl +
        this.endpoint +
        '/setssparea?id=' +
        id +
        '&sspareaid=' +
        sspAreaId,
      null
    );
  }

  setSspStopDate(id: string, date: Date): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl + this.endpoint + '/setsspstopdate/' + id,
      date
    );
  }

  setSocialWorker(id: string, socialWorker: string): Observable<boolean> {
    let socialworkerUrlEncoded = encodeURIComponent(socialWorker);

    return this.getHTTPService().put<boolean>(
      `${this.baseUrl}${this.endpoint}/setsocialworker?id=${id}&socialWorker=${socialworkerUrlEncoded}`,
      null
    );
  }

  deleteSspStopDate(id: string): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl + this.endpoint + '/deletesspstopdate/' + id,
      null
    );
  }
}
