import { Component, Directive, EventEmitter, Input, OnInit, Output, QueryList, ViewChildren } from '@angular/core';
import { DeathEntryDto } from '../dto/DeathEntryDto';
import { WikiDataItemDto } from '../dto/WikiDataItemDto';
import { WikipediaArticleDto } from '../dto/WikipediaArticleDto';

export type SortColumn = keyof DeathEntryDto | '';
export type SortDirection = 'asc' | 'desc' | '';
const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };

const compare = (v1: string | number | boolean | WikiDataItemDto | WikipediaArticleDto, v2: string | number | boolean | WikiDataItemDto | WikipediaArticleDto) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

export interface SortEvent {
  column: SortColumn;
  direction: SortDirection;
}

@Directive({
  selector: 'th[sortable]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()'
  }
})
export class NgbdSortableHeader {

  @Input() sortable: SortColumn = '';
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent>();

  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({column: this.sortable, direction: this.direction});
  }
}

@Component({
  selector: 'app-wikidata-details',
  templateUrl: './wikidata-details.component.html',
  styleUrls: ['./wikidata-details.component.css']
})
export class WikidataDetailsComponent {

  @Input() deathEntries: DeathEntryDto[];
  @Input() deathEntriesSorted: DeathEntryDto[];
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;

onSort({column, direction}: SortEvent) {
    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';        
      }
    });

    if (direction === '' || column === '') {
      this.deathEntriesSorted = this.deathEntries
    } else {
      this.deathEntriesSorted = [...this.deathEntries].sort((a, b) => {
        const res =  compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }
}