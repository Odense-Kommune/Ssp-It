import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
  OnChanges,
} from '@angular/core';
import { Subject } from 'rxjs';
import { Person } from '@models/person.model';
import { PersonService } from '@services/person.service';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-add-person',
  templateUrl: './add-person.component.html',
  styleUrls: ['./add-person.component.scss'],
})
export class AddPersonComponent implements OnInit, OnChanges {
  @Output() addPersonEvent: EventEmitter<any>;
  @ViewChild('personInput', { static: true }) personInput!: ElementRef;
  searchSubject: Subject<string> = new Subject<string>();
  searchedPersons: Person[];

  constructor(private personService: PersonService) {
    this.searchedPersons = new Array<Person>();
    this.addPersonEvent = new EventEmitter<any>();
  }

  ngOnInit() {
    this.searchSubject
      .pipe(debounceTime(200))
      .subscribe((t) => this.updatePersonList(t));
  }

  ngOnChanges() {
    this.closeSearch();
  }

  findPerson(searchTerm: string) {
    this.searchSubject.next(searchTerm);
  }

  closeSearch() {
    this.searchedPersons = [];
    this.personInput.nativeElement.value = '';
  }

  updatePersonList(searchTerm: string) {
    if (searchTerm.length === 0) {
      this.searchedPersons = [];
      return;
    }

    if (searchTerm.match(/^[0-9]/g)) {
      this.personService
        .searchByCpr(searchTerm)
        .subscribe((r) => (this.searchedPersons = r));
    } else {
      this.personService
        .searchByNameOrGroup(searchTerm)
        .subscribe((r) => (this.searchedPersons = r));
    }
  }

  addPersonToCurrentGroup(person: Person, event: any) {
    this.addPersonEvent.emit(person);
    event.srcElement.innerHTML = 'Tilføjet';
    event.srcElement.setAttribute('disabled', '');
    event.srcElement.classList.add('btn-success');
    event.srcElement.classList.remove('btn-default');
  }
}
