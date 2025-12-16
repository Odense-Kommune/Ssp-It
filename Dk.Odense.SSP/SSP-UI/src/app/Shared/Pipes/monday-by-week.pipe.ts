import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'mondayByWeek',
})
export class MondayByWeekPipe implements PipeTransform {
  transform(value: number, date: any): any {
    let isoDate: any;
    if (typeof date === 'string')
      isoDate = this.getDateOfISOWeek(value, date.substring(0, 4));
    else isoDate = this.getDateOfISOWeek(value, date.getFullYear());
    return 'Uge ' + value + ': ' + isoDate.toLocaleDateString();
  }

  getDateOfISOWeek(weekNumber: number, year: any) {
    const simple = new Date(year, 0, 1 + (weekNumber - 1) * 7);
    const dayOfWeek = simple.getDay();
    const ISOweekStart = simple;
    if (dayOfWeek <= 4)
      ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    else ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());
    return ISOweekStart;
  }
}
