import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { Worry } from '@domain-models/worry.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WorryService extends BaseService<Worry> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'worry';
  }

  setPoliceWorryRole(
    id: string,
    policeWorryRoleId: string
  ): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl +
        this.endpoint +
        '/setpoliceworryrole?id=' +
        id +
        '&policeworryroleid=' +
        policeWorryRoleId,
      ''
    );
  }

  setPoliceWorryCategory(
    id: string,
    policeWorryCategoryId: string
  ): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl +
        this.endpoint +
        '/setpoliceworrycategory?id=' +
        id +
        '&policeworrycategoryid=' +
        policeWorryCategoryId,
      ''
    );
  }

  getNext(personId: string, increment: number): Observable<Worry> {
    return this.getHTTPService().get<Worry>(
      this.baseUrl +
        this.endpoint +
        '/GetNext?personId=' +
        personId +
        '&increment=' +
        increment
    );
  }

  getPrevious(personId: string, increment: number): Observable<Worry> {
    return this.getHTTPService().get<Worry>(
      this.baseUrl +
        this.endpoint +
        '/GetPrevious?personId=' +
        personId +
        '&increment=' +
        increment
    );
  }
}
