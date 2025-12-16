import { Injectable } from '@angular/core';
import { Grouping } from '@domain-models/grouping.model';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { MenuGrouping } from '@models/menu-grouping.model';
import { PersonGroups } from '@models/person-groups.model';
import { Person } from '@models/person.model';
import { PersonGrouping } from '@domain-models/person-grouping.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GroupingService extends BaseService<Grouping> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'grouping';
  }

  public searchGroupings(query: string): Observable<Grouping[]> {
    return this.getHTTPService().get<Grouping[]>(
      this.baseUrl + this.endpoint + '/SearchGroupings?query=' + query
    );
  }

  public getPeopleForGroup(group: MenuGrouping) {
    return this.getHTTPService().get<PersonGroups[]>(
      this.baseUrl + this.endpoint + '/GetPeopleForGroup?groupId=' + group.id
    );
  }

  public addPersonToGrouping(person: Person, group: Grouping) {
    const dto: PersonGrouping = {
      grouping_Id: group.id,
      person_Id: person.id,
    };
    return this.getHTTPService().post<any>(
      this.baseUrl + this.endpoint + '/addPersonToGroup',
      dto
    );
  }

  public removePersonFromGrouping(personId: string, groupId: string) {
    return this.getHTTPService().delete<any>(
      this.baseUrl +
        this.endpoint +
        '/deleteFromGroup?personId=' +
        personId +
        '&groupId=' +
        groupId
    );
  }

  public setClassification(
    person_Id: string,
    grouping_id: string,
    classification_id: string
  ) {
    return this.getHTTPService().post<PersonGroups>(
      this.baseUrl +
        this.endpoint +
        '/SetClassification?personId=' +
        person_Id +
        '&groupid=' +
        grouping_id +
        '&classificationId=' +
        classification_id,
      ''
    );
  }

  public menuGroupingList() {
    return this.getHTTPService().get<MenuGrouping[]>(
      this.baseUrl + this.endpoint + '/menugroupinglist'
    );
  }

  public menuPsuGroupingList() {
    return this.getHTTPService().get<MenuGrouping[]>(
      this.baseUrl + this.endpoint + '/menupsugroupinglist'
    );
  }

  public postPsu(item: Grouping): Observable<Grouping> {
    return this.getHTTPService().post<Grouping>(
      this.baseUrl + this.endpoint + '/postpsu',
      item
    );
  }

  public getGroupingStats(id: string) {
    return this.getHTTPService().get<any>(
      this.baseUrl + this.endpoint + '/GetGroupingStats?id=' + id
    );
  }
}
