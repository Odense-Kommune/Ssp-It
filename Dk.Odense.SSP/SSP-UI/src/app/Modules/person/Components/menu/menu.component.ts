import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Dictionary } from '@models/dictionary.model';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  @Input() menuItem!: Dictionary;

  @Output() pushWorryId = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  pushSelectedWorry() {
    this.pushWorryId.emit(this.menuItem.id);
  }
}
