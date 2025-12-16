import { Injectable } from '@angular/core';
import { VerifyWorryMenuItem } from '@models/verify-worry-menu-item.model';

import { HttpClient } from '@angular/common/http';
import { BaseService } from '@services/base.service';

@Injectable({
  providedIn: 'root',
})
export class VerifyWorryMenuItemService extends BaseService<VerifyWorryMenuItem> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'verifyworry';
  }
}
