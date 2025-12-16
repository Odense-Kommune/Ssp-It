import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AreaRule } from '@domain-models/area-rule.model';

@Injectable({
  providedIn: 'root',
})
export class AreaRuleService extends BaseService<AreaRule> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'arearule';
  }

  public del(item: AreaRule): Observable<AreaRule> {
    const http = this.getHTTPService();
    return http.delete<AreaRule>(
      this.baseUrl + this.endpoint + '/delete?id=' + item.id
    );
  }
}
