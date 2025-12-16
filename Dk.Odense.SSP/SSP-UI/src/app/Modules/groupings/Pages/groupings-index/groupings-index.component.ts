import { Component, OnInit } from '@angular/core';
import { MenuGrouping } from '@models/menu-grouping.model';
import { PersonGroups } from '@models/person-groups.model';
import { GroupingService } from '@services/grouping.service';
import { ActivatedRoute } from '@angular/router';
import { Grouping } from '@domain-models/grouping.model';
import { Person } from '@models/person.model';
import { GroupingType } from '@models/grouping-type';
import { Classification } from '@domain-models/classification.model';
import { ClassificationService } from '@services/classification.service';
import { GuidNull } from '@shared/Constants';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { GroupingModalComponent } from '../../components/grouping-modal/grouping-modal.component';

@Component({
  selector: 'app-groupings-index',
  templateUrl: './groupings-index.component.html',
  styleUrls: ['./groupings-index.component.scss'],
})
export class GroupingsIndexComponent implements OnInit {
  selectedGroup = new MenuGrouping();
  groupingsType: string | undefined;
  groups = new Array<MenuGrouping>();
  people = new Array<PersonGroups>();
  classifications = new Array<Classification>();
  groupingStats: any;
  editTitle = false;

  modalRef: MdbModalRef<GroupingModalComponent> | null = null;

  constructor(
    private groupingService: GroupingService,
    private classificationService: ClassificationService,
    private route: ActivatedRoute,
    private modalService: MdbModalService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.groupingsType = GroupingType[data['type']];
    });

    this.route.params.subscribe((r) => {
      const params = this.route.snapshot.params;
      this.updateData(params['id']);
    });

    this.getClassifications();
  }

  openGroupingModal() {
    this.modalRef = this.modalService.open(GroupingModalComponent, {
      modalClass: 'modal-lg',
      data: {
        groupId: this.selectedGroup.id,
        persons: this.people,
        groupingsType: this.groupingsType,
      },
    });
  }

  createGroup(event: Grouping) {
    switch (this.groupingsType) {
      case GroupingType['0']:
        this.groupingService.post(event).subscribe((s) => {
          this.groups.push({ id: s.id, personCount: 0, value: s.value });
        });
        break;

      case GroupingType['1']:
        this.groupingService.postPsu(event).subscribe((s) => {
          this.groups.push({ id: s.id, personCount: 0, value: s.value });
        });
        break;

      default:
        break;
    }
  }

  onAddPersonEvent(person: Person) {
    this.groupingService
      .addPersonToGrouping(person, this.selectedGroup)
      .subscribe((r) => this.getPeople());
  }

  deleteGroup(groupId: string, groupName: string) {
    if (confirm('Vil du slette ' + groupName)) {
      this.groupingService.delete(groupId).subscribe((r) => {
        this.getMenuItems();
      });
    }
  }

  updateData(route?: string) {
    if (route) {
      this.getMenuItems(route);
    } else {
      this.getMenuItems();
    }
  }

  getMenuItems(route?: string) {
    switch (this.groupingsType) {
      case GroupingType['0']:
        this.groupingService.menuGroupingList().subscribe((r) => {
          this.groups = r;

          if (route) {
            this.selectedGroup = this.getGroupById(route);
            this.getPeople();
            this.getGroupingStats();
          }
        });
        break;

      case GroupingType['1']:
        this.groupingService.menuPsuGroupingList().subscribe((r) => {
          this.groups = r;

          if (route) {
            this.selectedGroup = this.getGroupById(route);
            this.getPeople();
            this.getGroupingStats();
          }
        });
        break;
    }
  }

  getPeople() {
    this.groupingService
      .getPeopleForGroup(this.selectedGroup)
      .subscribe((r) => (this.people = r));
  }

  onPersonRemoved(person: PersonGroups) {
    this.groupingService
      .removePersonFromGrouping(person.person_Id, this.selectedGroup.id)
      .subscribe();
    this.removeFromPersonGroups(person);
  }

  removeFromPersonGroups(person: PersonGroups) {
    this.people.splice(
      this.people.findIndex((x) => x.person_Id === person.person_Id),
      1
    );
  }

  setClassification(personGroup: PersonGroups) {
    let classificationId = personGroup.classification_Id ?? GuidNull;

    this.groupingService
      .setClassification(
        personGroup.person_Id,
        this.selectedGroup.id,
        classificationId
      )
      .subscribe();
  }

  getGroupById(id: string) {
    let v = new MenuGrouping();
    this.groups.forEach((group) => {
      if (group.id === id) {
        v = group;
        return;
      }
    });
    return v;
  }

  getClassifications() {
    this.classificationService
      .getValidList()
      .subscribe((x: Classification[]) => {
        this.classifications = x;
      });
  }

  getGroupingStats() {
    this.groupingService
      .getGroupingStats(this.selectedGroup.id)
      .subscribe((x) => {
        this.groupingStats = x;
      });
  }

  toggleEdit() {
    switch (this.editTitle) {
      case true:
        let group = this.selectedGroup;
        this.groupingService.put(group).subscribe((x) => {});
        this.editTitle = false;
        break;

      case false:
        this.editTitle = true;
        break;

      default:
        break;
    }
  }
}
