import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Robustness } from '@domain-models/robustness.model';

@Injectable({
  providedIn: 'root',
})
export class RobustnessService extends BaseService<Robustness> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'robustness';
  }

  getNext(personId: string, increment: number): Observable<Robustness> {
    return this.getHTTPService().get<Robustness>(
      this.baseUrl +
        this.endpoint +
        '/getnextbyperson?personId=' +
        personId +
        '&increment=' +
        increment
    );
  }

  getPrevious(personId: string, increment: number): Observable<Robustness> {
    return this.getHTTPService().get<Robustness>(
      this.baseUrl +
        this.endpoint +
        '/getpreviousbyperson?personId=' +
        personId +
        '&increment=' +
        increment
    );
  }
}
