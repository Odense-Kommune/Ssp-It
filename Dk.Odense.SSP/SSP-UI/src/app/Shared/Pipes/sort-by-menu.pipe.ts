import { Pipe, PipeTransform } from '@angular/core';
import { VerifyWorryMenuItem } from '@models/verify-worry-menu-item.model';

@Pipe({
  name: 'sortVerifyWorryMenuItem',
})
export class SortVerifyWorryMenuItem implements PipeTransform {
  transform(items: VerifyWorryMenuItem[]): any {
    return items.sort((a, b) => {
      if (
        a.source == null ||
        b.source == null ||
        a.source.localeCompare(b.source) === 0
      )
        return Number.parseInt(a.increment) - Number.parseInt(b.increment);
      return a.source.localeCompare(b.source);
    });
  }
}
