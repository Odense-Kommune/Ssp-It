import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { PendingWorry } from '@models/pending-worry.model';
import { Agenda } from '@models/agenda.model';
import { ToastService } from '@services/toast.service';

@Component({
  selector: 'app-pending-worry',
  templateUrl: './pending-worry.component.html',
  styleUrls: ['./pending-worry.component.scss'],
})
export class PendingWorryComponent implements OnInit, OnChanges {
  @Input() pendingWorries!: PendingWorry[];
  @Input() agendas!: Agenda[];
  @Input() busy!: boolean;
  @Output() addWorriesEvent = new EventEmitter<PendingWorry[]>();
  loaded: boolean;
  constructor(private toastSerivce: ToastService) {
    this.loaded = false;
  }

  ngOnInit() {}

  ngOnChanges() {
    if (!this.loaded) {
      this.loaded = !this.loaded;
    }
  }

  checkboxClick(event: any, worry: PendingWorry) {
    const src = event.srcElement;
    worry.checked = !worry.checked;
    if (src.nodeName === 'INPUT') {
      return;
    }
    const input: any = src.parentElement.classList.contains('content-item')
      ? src.parentElement.getElementsByTagName('INPUT')[0]
      : src.getElementsByTagName('INPUT')[0];
    input.checked = worry.checked;
  }

  addNewWorries(items: PendingWorry[]) {
    this.addWorriesEvent.emit(items);
  }

  getSelectedWorries(): PendingWorry[] {
    return this.pendingWorries.filter((p) => p.checked);
  }

  addSelectedToAgenda(agendaId: string) {
    const selectedWorries = this.getSelectedWorries();
    if (agendaId === '') {
      this.toastSerivce.createToast(
        'Der er ikke valgt nogen dagsorden',
        'danger'
      );
      return;
    }
    if (selectedWorries.length === 0) {
      this.toastSerivce.createToast(
        'Der er ikke valgt nogen punkter',
        'danger'
      );
      return;
    }
    selectedWorries.forEach((e) => (e.agendaId = agendaId));
    this.addNewWorries(selectedWorries);
  }

  getAgenda(id: string): Agenda {
    let el = new Agenda();
    this.agendas.forEach((e) => {
      if (e.id === id) {
        el = e;
        return;
      }
    });
    return el;
  }
}
