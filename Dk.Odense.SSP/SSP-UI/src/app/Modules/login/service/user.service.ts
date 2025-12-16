import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseService<boolean> {
  public isLoggedIn = false;

  constructor(http: HttpClient, private router: Router) {
    super(http);
    this.endpoint = 'microsoftidentity/account';
  }

  isAuthorized(): boolean {
    this.getHTTPService()
      .get<boolean>(environment.baseUrl2 + 'account/IsAuthorized')
      .subscribe(
        (r) => {
          /*if (r == false) {
            window.location.href =
              environment.baseUrl2 + this.endpoint + '/signin';
          }*/

          this.isLoggedIn = true;
          var url = sessionStorage.getItem('url');
          sessionStorage.removeItem('url');
          this.router.navigate([url]);
        },
        (error) => {
          window.location.href =
            environment.baseUrl2 + this.endpoint + '/signin';
          this.isLoggedIn = false;
        }
      );
    return this.isLoggedIn;
  }
}
