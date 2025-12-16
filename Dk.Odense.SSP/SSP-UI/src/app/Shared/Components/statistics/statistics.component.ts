import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { Dictionary } from '@models/dictionary.model';
import { GuidNull } from '../../Constants';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss'],
})
export class StatisticsComponent implements OnChanges, OnInit {
  @Input() policeWorryCategories!: Dictionary[];
  @Input() policeWorryRoles!: Dictionary[];
  @Input() selectedRole?: string;
  @Input() selectedCategory?: string;

  @Output() pushPoliceCategory = new EventEmitter<string>();
  @Output() pushPoliceRole = new EventEmitter<string>();

  guidNul = GuidNull;

  constructor() {}
  ngOnInit() {}
  setPoliceWorryCategory(event: Event) {
    let id = (event.target as HTMLSelectElement).value;
    if (id !== GuidNull) {
      this.pushPoliceCategory.emit(id);
    }
  }

  setPoliceWorryRole(event: Event) {
    let id = (event.target as HTMLSelectElement).value;
    if (id !== GuidNull) {
      this.pushPoliceRole.emit(id);
    }
  }

  ngOnChanges() {
    if (this.selectedCategory === null) {
      this.selectedCategory = this.guidNul;
    }
    if (this.selectedRole === null) {
      this.selectedRole = this.guidNul;
    }
  }
}
