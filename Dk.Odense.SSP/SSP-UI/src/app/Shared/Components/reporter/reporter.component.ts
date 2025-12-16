import { Component, OnInit, Input } from '@angular/core';
import { Reporter } from '@domain-models/reporter.model';

@Component({
  selector: 'app-reporter',
  templateUrl: './reporter.component.html',
  styleUrls: ['./reporter.component.scss'],
})
export class ReporterComponent implements OnInit {
  @Input() reporter!: Reporter | undefined;
  constructor() {}

  ngOnInit() {}
}
