import { Component, OnInit } from '@angular/core';
import { Person } from '@models/person.model';
import { PersonService } from '@services/person.service';
import { Grouping } from '@domain-models/grouping.model';
import { GroupingService } from '@services/grouping.service';

@Component({
  selector: 'app-person-index',
  templateUrl: './person-index.component.html',
  styleUrls: ['./person-index.component.scss'],
})
export class PersonIndexComponent implements OnInit {
  persons = new Array<Person>();
  groups = new Array<Grouping>();
  filterVal = '';
  sspStopFilter = '';
  loading = true;

  constructor(
    private personService: PersonService,
    private groupingService: GroupingService
  ) {}

  getPersons(): void {
    this.personService.list().subscribe((persons) => {
      this.persons = persons;
      this.loading = false;
    });
  }

  ngOnInit() {
    this.getPersons();
    this.groupingService.list().subscribe((x) => (this.groups = x));
  }
}
