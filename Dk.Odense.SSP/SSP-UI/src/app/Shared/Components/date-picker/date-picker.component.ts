import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss'],
})
export class DatePickerComponent implements OnInit, OnChanges {
  public myDatePickerOptions = {
    // Your options
    title: 'Vælg Dato',
    weekdaysFull: [
      'Søndag',
      'Mandag',
      'Tirsdag',
      'Onsdag',
      'Torsdag',
      'Fredag',
      'Lørdag',
    ],
    weekdaysShort: ['Søn', 'Man', 'Tirs', 'Ons', 'Tors', 'Fre', 'Lør'],
    weekdaysNarrow: ['S', 'M', 'T', 'O', 'T', 'F', 'L'],
    monthsFull: [
      'Januar',
      'Februar',
      'Marts',
      'April',
      'Maj',
      'Juni',
      'Juli',
      'August',
      'September',
      'Oktober',
      'November',
      'December',
    ],
    monthsShort: [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'Maj',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Okt',
      'Nov',
      'Dec',
    ],
    firstDayOfWeek: 'mo',
    todayBtnTxt: 'd.d.',
    clearBtnText: 'Slet',
    cancelBtnText: 'Stop',
  };

  startDate!: Date | null;

  @Input() date!: Date | null;

  @Output() pushDate = new EventEmitter<Array<Date>>();

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    if (!this.checkValidDate(this.date)) {
      this.date = this.convertAndSetToJsDate(this.date);
      this.startDate = this.date;
    } else {
      if (this.date != null) {
        this.date = this.removeTime(this.date);
      }
      this.startDate = this.date;
    }
  }

  ngOnInit() {}
  removeTime(date: Date) {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }

  checkValidDate(d: any) {
    const dateString = Date.parse(d);
    if (d instanceof Date && !isNaN(dateString)) return true;
    else return false;
  }
  convertAndSetToJsDate(date: any): Date | null {
    if (date === null || date === undefined) return null;

    let dateStr: string = date;
    //Remove timestamp
    let x = dateStr.split('T');
    const d = x[0].split('-');
    //Convert to yyyy-mm-dd from dd-mm-yyyy
    let year = Number(d[0]);
    let month = Number(d[1]);
    let day = Number(d[2]);

    const newDate = new Date(year, month - 1, day);

    if (this.checkValidDate(new Date(newDate))) {
      return new Date(newDate);
    }
    //Return new date in yyyy-mm-dd
    return new Date(`${d[0]}/${d[1]}/${d[2]}`);
  }

  datePicked(event: Date): Date[] {
    var dates = new Array<Date>();
    dates.push(event);
    this.pushDate.emit(dates);
    this.startDate = new Date(event);
    return dates;
  }
}
