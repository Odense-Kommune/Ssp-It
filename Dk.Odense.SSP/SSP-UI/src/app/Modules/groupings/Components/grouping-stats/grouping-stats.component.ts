import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-grouping-stats',
  templateUrl: './grouping-stats.component.html',
  styleUrls: ['./grouping-stats.component.scss'],
})
export class GroupingStatsComponent implements OnInit {
  @Input() groupingStats: any;

  constructor() {}

  ngOnInit() {}
}
