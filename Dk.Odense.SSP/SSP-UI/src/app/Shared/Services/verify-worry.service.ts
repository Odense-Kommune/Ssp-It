import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VerifyWorryMenuItem } from '@models/verify-worry-menu-item.model';
import { Observable } from 'rxjs';
import { VerifyWorry } from '@models/verify-worry.model';
import { BaseService } from '@services/base.service';
import { VerifyWorryMenuItemService } from '@services/verify-worry-menu-item.service';

@Injectable({
  providedIn: 'root',
})
export class VerifyWorryService extends BaseService<VerifyWorry> {
  constructor(
    http: HttpClient,
    private menuService: VerifyWorryMenuItemService
  ) {
    super(http);
    this.endpoint = 'verifyworry';
  }

  getMenuItems(): Observable<VerifyWorryMenuItem[]> {
    return this.menuService.listFromEndpoint('getmenuitems');
  }

  getVerifyWorry(id: string): Observable<VerifyWorry> {
    return this.get(id);
  }

  setGroundless(id: string): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl + this.endpoint + '/setgroundless/' + id,
      ''
    );
  }

  setApproved(id: string, socialSecNum: string): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl +
        this.endpoint +
        '/setapproved/' +
        id +
        '?socialsecnum=' +
        socialSecNum,
      ''
    );
  }

  unverify(id: string): Observable<boolean> {
    return this.getHTTPService().put<boolean>(
      this.baseUrl + this.endpoint + '/unverify/' + id,
      ''
    );
  }

  getPendingNumber(): Observable<number> {
    return this.getHTTPService().get<number>(
      this.baseUrl + this.endpoint + '/getPendingWorries'
    );
  }
}
