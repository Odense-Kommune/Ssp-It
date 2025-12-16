import {
  Component,
  OnInit,
  Input,
  OnChanges,
  Output,
  EventEmitter,
} from '@angular/core';
import { MenuGrouping } from '@models/menu-grouping.model';
import { GroupingService } from '@services/grouping.service';
import { Grouping } from '@domain-models/grouping.model';
import { ModalService } from '@services/modal.service';
import { GroupingType } from '@models/grouping-type';
import { ConstantPool } from '@angular/compiler';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})
export class FilterComponent implements OnInit, OnChanges {
  @Input() groupingsType: string | undefined;
  @Input() groups: MenuGrouping[];
  @Output() newGroupEvent: EventEmitter<Grouping>;
  linkRoute = '';
  filterText: string;
  grpSuccess: boolean;

  constructor(private modal: ModalService) {
    this.newGroupEvent = new EventEmitter<Grouping>();
    this.groups = new Array<MenuGrouping>();
    this.filterText = '';
    this.grpSuccess = false;
  }

  ngOnInit() {
    switch (this.groupingsType) {
      case GroupingType['0']:
        this.linkRoute = '/grouping';
        break;

      case GroupingType['1']:
        this.linkRoute = '/psu';
        break;

      default:
        break;
    }
  }

  ngOnChanges() {
    if (this.groups.length != 0) {
      this.groups.sort((a, b) => {
        if (a.value.toLocaleLowerCase() > b.value.toLocaleLowerCase()) {
          return 1;
        }
        if (b.value.toLocaleLowerCase() > a.value.toLocaleLowerCase()) {
          return -1;
        }
        return 0;
      });
    }
  }

  getFilteredGroupings(): MenuGrouping[] {
    if (this.filterText.length === 0) {
      return this.groups;
    }
    return this.groups.filter((g) => {
      return (
        g.value
          .toLocaleLowerCase()
          .indexOf(this.filterText.toLocaleLowerCase()) > -1
      );
    });
  }

  createGroup(groupName: string) {
    if (groupName === '') return;
    const group = new Grouping();
    group.value = groupName;

    this.newGroupEvent.emit(group);
    /*this.groupingService.post(group).subscribe((s) => {
      this.grpSuccess = true;
      this.groups.push({ id: s.id, personCount: 0, value: s.value });
      setTimeout(() => {
        this.grpSuccess = false;
      }, 5000);
    });*/
  }
}
