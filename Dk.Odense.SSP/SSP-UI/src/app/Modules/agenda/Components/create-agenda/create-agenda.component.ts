import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Agenda } from '@models/agenda.model';

@Component({
  selector: 'app-create-agenda',
  templateUrl: './create-agenda.component.html',
  styleUrls: ['./create-agenda.component.scss'],
})
export class CreateAgendaComponent implements OnInit {
  @Output() newAgenda = new EventEmitter<Agenda>();
  public myDatePickerOptions = {
    // Your options
  };

  // Form Controls
  meetingDate: Date;
  agendaName: string | undefined;

  onSubmit() {
    const agenda = new Agenda();
    agenda.date = this.meetingDate;
    agenda.agendaName = this.agendaName;
    agenda.agendaSent = false;
    agenda.meetingHeld = false;
    this.newAgenda.emit(agenda);
  }

  constructor() {
    this.meetingDate = new Date();
  }

  ngOnInit() {}

  onDateUpdated(date: Date[]) {
    this.meetingDate = date[0];
  }
}
