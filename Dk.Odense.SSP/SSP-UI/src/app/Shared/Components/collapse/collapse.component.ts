import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-collapse',
  templateUrl: './collapse.component.html',
  styleUrls: ['./collapse.component.scss'],
})
export class CollapseComponent implements OnInit {
  @Input() title: string | undefined;
  @Input() preTitle!: string;
  @Input() collapsed!: boolean;
  @ViewChild('inputRef', { static: true }) input!: ElementRef;
  isCollapsed?: boolean;

  constructor() {}

  ngOnInit() {
    if (this.collapsed) {
      this.isCollapsed = true;
    } else {
      this.isCollapsed = false;
    }
  }

  collapse() {
    this.isCollapsed = !this.isCollapsed;
  }
}
