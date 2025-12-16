import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Robustness } from '@domain-models/robustness.model';
import { Concern } from '@domain-models/concern.model';
import { RobustnessService } from '@services/robustness.service';
import { MaxInt } from '../../Constants';

@Component({
  selector: 'app-page-robustness',
  templateUrl: './page-robustness.component.html',
  styleUrls: ['./page-robustness.component.scss'],
})
export class PageRobustnessComponent implements OnInit, OnChanges {
  @Input() personId!: string;
  robustness?: Robustness;
  concern?: Concern;

  constructor(private robustnessService: RobustnessService) {}

  ngOnInit() {}

  ngOnChanges() {
    this.resetData();
    this.onPrevious();
  }

  onNext(): void {
    const increment =
      this.robustness == null ? MaxInt : this.robustness.increment;
    this.robustnessService.getNext(this.personId, increment).subscribe((x) => {
      if (x) {
        this.robustness = x;
      }
    });
  }

  onPrevious(): void {
    const increment =
      this.robustness == null ? MaxInt : this.robustness.increment;
    this.robustnessService
      .getPrevious(this.personId, increment)
      .subscribe((x) => {
        if (x) {
          this.robustness = x;
        }
      });
  }

  resetData() {
    this.robustness = undefined;
  }
}
