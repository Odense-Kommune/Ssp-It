import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sortBy'
})

export class SortBy implements PipeTransform {
  transform(items: any[], sortVal: string): any {
    if (!items) return false;
    if (!sortVal) return items;
    return items.sort((a: any, b: any) => {
      let aVal;
      let bVal;

      for (const key in a) {
        if (key === sortVal) {
          aVal = a[key];
        }
      }

      for (const key in b) {
        if (key === sortVal) {
          bVal = b[key];
        }
      }
      
      if (aVal < bVal) return -1;
      if (aVal > bVal) return 1;

      return 0;
    });
  }
}
