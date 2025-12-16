import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dynamicFilter',
})
export class DynamicFilter implements PipeTransform {
  transform(items: any[], filterVal: any): any {
    if (!items) {
      return false;
    }
    if (!filterVal) {
      return items;
    }

    return items.filter((item) => {
      for (const key in item) {
        if (key !== 'id') {
          if (
            ('' + item[key]).toLowerCase().includes(filterVal.toLowerCase())
          ) {
            return true;
          }
          if (typeof item[key] === 'object') {
            for (const subkey in item[key]) {
              if (subkey !== 'id') {
                if (
                  ('' + item[key][subkey])
                    .toLowerCase()
                    .includes(filterVal.toLowerCase())
                ) {
                  return true;
                }
                if (typeof item[key][subkey] === 'object') {
                  for (const subsubkey in item[key][subkey]) {
                    if (subsubkey !== 'id') {
                      if (
                        ('' + item[key][subkey][subsubkey])
                          .toLowerCase()
                          .includes(filterVal.toLowerCase())
                      ) {
                        return true;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      return false;
    });
  }
}
