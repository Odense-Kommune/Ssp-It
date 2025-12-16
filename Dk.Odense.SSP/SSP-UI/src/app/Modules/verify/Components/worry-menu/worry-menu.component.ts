import { Component, OnInit, Input } from '@angular/core';
import { VerifyWorryMenuItem } from '@models/verify-worry-menu-item.model';

@Component({
  selector: 'app-worry-menu',
  templateUrl: './worry-menu.component.html',
  styleUrls: ['./worry-menu.component.scss'],
})
export class WorryMenuComponent implements OnInit {
  @Input() menuItem!: VerifyWorryMenuItem;

  constructor() {}

  ngOnInit() {}
}
