import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'shorten33603294' })
export class Shorten33603294Pipe implements PipeTransform {
  transform(value: string, maxLength: number): string {
    return value.length > maxLength ? value.slice(0, maxLength) + '...' : value;
  }
}
