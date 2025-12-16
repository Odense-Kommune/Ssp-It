import {
  Component,
  OnInit,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { Person } from '@models/person.model';
import { Grouping } from '@domain-models/grouping.model';
import { GroupingService } from '@services/grouping.service';
import { Observable, startWith, Subject, map, mapTo, tap } from 'rxjs';

@Component({
  selector: 'app-datacard',
  templateUrl: './datacard.component.html',
  styleUrls: ['./datacard.component.scss'],
})
export class DatacardComponent implements OnInit, OnChanges {
  @Input() person!: Person;
  @Input() groups!: Grouping[];
  results!: Observable<Grouping[]>;
  searchText = new Subject<string>();
  hidden = true;
  notFound = false;

  constructor(private groupingService: GroupingService) {}

  ngOnInit() {
    this.results = this.searchText.pipe(
      map((value: string) => this.filter(value)),
      tap((results: Grouping[]) =>
        results.length > 0 ? (this.notFound = false) : (this.notFound = true)
      )
    );
  }

  filter(value: string): Grouping[] {
    if (!value) return new Array<Grouping>();
    const filterValue = value.toLowerCase();
    if (this.groups) {
      return this.groups.filter((item: Grouping) =>
        item.value.toLowerCase().includes(filterValue)
      );
    }
    return new Array<Grouping>();
  }

  ngOnChanges() {}

  AddGroup(event: any) {
    const index = this.groups.findIndex((x) => x.id === event.option.value);
    const group = this.groups[index];
    this.groupingService
      .addPersonToGrouping(this.person, group)
      .subscribe((x) => {
        this.person.groupings.push(group);
        this.searchText.next('');
        this.hidden = true;
      });
  }

  RemoveGrouping(grouping: Grouping) {
    if (confirm(`Er du sikker på du vil fjerne "${grouping.value}"?`)) {
      const index = this.person.groupings.findIndex(
        (x) => x.id === grouping.id
      );
      this.groupingService
        .removePersonFromGrouping(this.person.id, grouping.id)
        .subscribe((x) => {
          this.person.groupings.splice(index, 1);
        });
    }
  }

  Show() {
    this.hidden = !this.hidden;
  }

  displayValue(value: any): string {
    return value ? value.name : '';
  }
}
