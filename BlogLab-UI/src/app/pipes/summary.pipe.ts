import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'summary'
})
export class SummaryPipe implements PipeTransform {

  transform(content: string, characterCount: number): string {
    if (content.length <= characterCount) {
      return content;
    } else {
      return `${content.substring(0, characterCount)}...`;
    }

  }

}
