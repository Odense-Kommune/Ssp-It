import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { Dictionary } from '@models/dictionary.model';
import { GuidNull } from '@shared/Constants';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.scss'],
})
export class ActionsComponent implements OnInit, OnChanges {
  @Input() categorizations!: Dictionary[];
  @Input() sspAreas!: Dictionary[];

  @Input() selectedCategorization!: string;
  @Input() selectedSspArea!: string | undefined;

  @Input() socialWorker?: string;

  @Output() pushCategorization = new EventEmitter<string>();
  @Output() pushSspArea = new EventEmitter<string>();

  @Output() pushSocialWorker = new EventEmitter<string>();

  constructor() {}

  setCategorization(event: Event) {
    let id = (event.target as HTMLSelectElement).value;
    if (id !== GuidNull) {
      this.pushCategorization.emit(id);
    }
  }

  setSspArea(event: Event) {
    let id = (event.target as HTMLSelectElement).value;

    if (id === '') {
      if (!confirm('Er du sikker på du vil fjerne Sagsområdet?')) return;
      id = GuidNull;
    }

    this.pushSspArea.emit(id);
  }

  setSocialWorker() {
    this.pushSocialWorker.emit(this.socialWorker);
  }

  ngOnChanges() {
    if (
      !this.selectedCategorization ||
      this.selectedCategorization === GuidNull
    ) {
      this.selectedCategorization = '';
    }
    if (!this.selectedSspArea || this.selectedSspArea === GuidNull) {
      this.selectedSspArea = '';
    }
  }

  ngOnInit() {}
}
