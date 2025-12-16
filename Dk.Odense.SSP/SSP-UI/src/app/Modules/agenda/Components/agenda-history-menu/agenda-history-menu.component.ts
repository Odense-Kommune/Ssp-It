import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-agenda-history-menu',
  templateUrl: './agenda-history-menu.component.html',
  styleUrls: ['./agenda-history-menu.component.scss'],
})
export class AgendaHistoryMenuComponent implements OnInit {
  @Input() items!: any[];

  constructor() {}

  ngOnInit() {}
}
