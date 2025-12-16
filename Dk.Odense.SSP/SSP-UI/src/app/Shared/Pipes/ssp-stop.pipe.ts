import { Pipe, PipeTransform } from '@angular/core';
import { Person } from '@models/person.model';

@Pipe({
  name: 'sspStop',
})
export class SspStopPipe implements PipeTransform {
  transform(items: Person[], filterVal: string): any {
    return items.filter((item) => {
      switch (filterVal) {
        case 'yes':
          if (item.sspStopDate) return true;
          return false;
        case 'no':
          if (!item.sspStopDate) return true;
          return false;
        default:
          return true;
      }
    });
  }
}
