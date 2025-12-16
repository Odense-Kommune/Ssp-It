import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { MapAnswer } from '@models/answer.enum';
import { Concern } from '@domain-models/concern.model';

@Component({
  selector: 'app-concern',
  templateUrl: './concern.component.html',
  styleUrls: ['./concern.component.scss'],
})
export class ConcernComponent implements OnInit, OnChanges {
  @Input() concern?: Concern;

  constructor() {}
  ngOnInit() {}
  ngOnChanges() {
    this.mapConcern();
  }

  mapConcern() {
    if (this.concern) {
      const mapAnswer = new MapAnswer();

      this.concern.notifyConcern = mapAnswer.map(
        parseInt(this.concern.notifyConcern)
      );
      this.concern.reportedToPolice = mapAnswer.map(
        parseInt(this.concern.reportedToPolice)
      );
    }
  }
}
