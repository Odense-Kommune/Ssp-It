import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
} from '@angular/core';

@Component({
  selector: 'app-pending-status',
  templateUrl: './pending-status.component.html',
  styleUrls: ['./pending-status.component.scss'],
})
export class PendingStatusComponent implements OnInit {
  @Input() PendingItems!: number;
  @Output() updateDataEvent: EventEmitter<string>;
  @ViewChild('spin', { static: true }) spin?: ElementRef;
  @ViewChild('btn', { static: true }) btn?: ElementRef;
  constructor() {
    this.updateDataEvent = new EventEmitter<string>();
  }

  ngOnInit() {}

  updateData() {
    const el = this.btn?.nativeElement;
    el.setAttribute('disabled', 'true');
    window.setTimeout(() => el.removeAttribute('disabled'), 2000);
    this.updateDataEvent.emit('update');
    this.animate();
  }

  animate() {
    const el: HTMLElement = this.spin?.nativeElement;
    if (el.classList.contains('spin')) {
      return;
    }
    el.classList.add('spin');
    window.setTimeout(() => el.classList.remove('spin'), 2000);
  }
}
