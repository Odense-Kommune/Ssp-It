import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Dictionary } from '@models/dictionary.model';
import { Note } from '@domain-models/note.model';
import { PersonMenuItem } from '@models/person-menu-item.model';
import { VerifyWorry } from '@models/verify-worry.model';
import { Person } from '@models/person.model';
import { ActivatedRoute } from '@angular/router';
import { SpecificPersonService } from '@services/specific-person.service';
import { PersonService } from '@services/person.service';
import { VerifyWorryService } from '@services/verify-worry.service';
import { PoliceWorryCategoryService } from '@services/police-worry-category.service';
import { PoliceWorryRoleService } from '@services/police-worry-role.service';
import { WorryService } from '@services/worry.service';
import { NoteService } from '@services/note.service';
import { GuidNull } from '@shared/Constants';
import { GroupingService } from '@services/grouping.service';
import { Grouping } from '@domain-models/grouping.model';
import { NoteAdditionalService } from '@services/note-additional.service';
import { NoteAdditional } from '@domain-models/note-additional.model';

@Component({
  selector: 'app-person-specific',
  templateUrl: './person-specific.component.html',
  styleUrls: ['./person-specific.component.scss'],
})
export class PersonSpecificComponent implements OnInit {
  @ViewChild('scrollable1', { static: true }) scroll1!: ElementRef;
  @ViewChild('scrollable2', { static: true }) scroll2!: ElementRef;

  policeWorryCategories?: Dictionary[];
  policeWorryRoles?: Dictionary[];
  personId: string;
  notes = new Array<Note>();
  additionalNotes = new Array<NoteAdditional>();
  note = new Note();
  additionalNote = new NoteAdditional();
  menuItems = new Array<PersonMenuItem>();
  notOnAgendaMenuItems = new Array<PersonMenuItem>();
  onAgendaMenuItems = new Array<PersonMenuItem>();
  verifyWorry?: VerifyWorry;
  person = new Person();
  selectedWorry = '';
  countNonAgendaMenu?: number;
  countOnAgendaMenu?: number;
  groups = new Array<Grouping>();

  constructor(
    private route: ActivatedRoute,
    private specificPersonService: SpecificPersonService,
    private personService: PersonService,
    private verifyWorryService: VerifyWorryService,
    private policeWorryCategoryService: PoliceWorryCategoryService,
    private policeWorryRoleService: PoliceWorryRoleService,
    private worryService: WorryService,
    private noteService: NoteService,
    private groupingService: GroupingService,
    private additionalNoteService: NoteAdditionalService
  ) {
    this.menuItems = [];
    this.personId = '';
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id != undefined || id != null) this.personId = id;
    this.countNonAgendaMenu = 0;
    this.countOnAgendaMenu = 0;
    this.groupingService.list().subscribe((x) => {
      this.groups = x;
    });
    this.getPerson();
    this.getMenuItems();
    this.getPoliceWorryCategories();
    this.getPoliceWorryRoles();
    this.getNotes();
    this.resetNote();
  }

  getPerson() {
    this.personService.get(this.personId).subscribe((x) => {
      this.person = x;
    });
  }

  getMenuItems() {
    this.specificPersonService.getMenuItems(this.personId).subscribe((x) => {
      this.menuItems = x;
      this.notOnAgendaMenuItems = this.notOnAgendaMenuItemsFilter();
      this.onAgendaMenuItems = this.onAgendaMenuItemsFilter();
    });
  }

  getVerifyWorry() {
    this.verifyWorryService.get(this.selectedWorry).subscribe((x) => {
      this.verifyWorry = x;
    });
  }

  getSelectedWorryId(event: Event) {
    this.selectedWorry = event + '';
    this.getVerifyWorry();
  }

  getPoliceWorryCategories(): void {
    this.policeWorryCategoryService
      .list()
      .subscribe((x) => (this.policeWorryCategories = x));
  }

  savePoliceWorryCategory(id: string): void {
    this.worryService
      .setPoliceWorryCategory(this.selectedWorry, id)
      .subscribe();
  }

  getPoliceWorryRoles(): void {
    this.policeWorryRoleService
      .list()
      .subscribe((x) => (this.policeWorryRoles = x));
  }

  savePoliceWorryRole(id: string): void {
    this.worryService.setPoliceWorryRole(this.selectedWorry, id).subscribe();
  }
  getNotes(): void {
    this.noteService.getNotesByPerson(this.personId).subscribe((notes) => {
      this.notes = notes;
    });
    this.additionalNoteService
      .getNotesByPerson(this.personId)
      .subscribe((notes) => (this.additionalNotes = notes));
  }

  onSaveNote(text: Note): void {
    this.note.value = text.value;
    this.note.person_id = this.person.id;
    this.noteService
      .saveNoteToPerson(this.note)
      .subscribe((x) => (this.note = x));
  }
  onSaveAddNote(text: Note): void {
    this.additionalNote.value = text.value;
    this.additionalNote.person_id = this.person.id;
    this.additionalNoteService
      .saveNoteToPerson(this.additionalNote)
      .subscribe((x) => (this.additionalNote = x));
  }

  saveSspStopDate(dates: Date[]) {
    const warningDate = new Date();
    warningDate.setDate(new Date().getDate() - 30);

    if (dates[0] < warningDate) {
      const conf = confirm(
        'Den ssp-stopdato, du taster er ældre end 30 dage. Er du sikker?'
      );

      if (conf) {
        this.personService.setSspStopDate(this.personId, dates[0]).subscribe();
      } else {
        if (dates[1]) {
          this.person.sspStopDate = dates[1];
        }
      }
    } else if (!this.person.categorization.deleteAfterSspEnd) {
      alert(
        'Borgerens kategorisering tager ikke hensyn til SSP-stopdato. Ret kategoriseringen, eller slet datoen igen'
      );
      this.personService.setSspStopDate(this.personId, dates[0]).subscribe();
    } else {
      this.personService.setSspStopDate(this.personId, dates[0]).subscribe();
    }
  }

  deleteSspStopDate() {
    this.personService.deleteSspStopDate(this.personId).subscribe((x) => {
      if (!x) {
        return;
      }
      this.person.sspStopDate = null;
    });
  }

  notOnAgendaMenuItemsFilter() {
    let filterMenuItem = new Array<PersonMenuItem>();
    filterMenuItem = this.menuItems.filter((x) => x.agendaId === GuidNull);
    this.countNonAgendaMenu = filterMenuItem.length;
    return filterMenuItem;
  }

  onAgendaMenuItemsFilter() {
    const filterMenuItem = this.menuItems.filter(
      (x) => x.agendaId !== GuidNull
    );
    this.countOnAgendaMenu = filterMenuItem.length;
    return filterMenuItem;
  }

  scrollableClick(event: any) {
    const scroll1: Element = this.scroll1.nativeElement;
    const scroll2: Element = this.scroll2.nativeElement;
    const src = this.getCorrectSource(event.srcElement)?.nextSibling;
    if (src === scroll1) {
      this.recollapse(scroll1);
    }
    if (src === scroll2) {
      this.recollapse(scroll2);
    }
  }

  recollapse(sourceElement: Element) {
    const scroll1: Element = this.scroll1.nativeElement;
    const scroll2: Element = this.scroll2.nativeElement;
    scroll1.classList.add('collapsed');
    scroll2.classList.add('collapsed');
    sourceElement.classList.remove('collapsed');
  }

  getCorrectSource(element: Element) {
    if (element.nodeName === 'H6') {
      return element.parentElement;
    }
    return element;
  }

  resetNote() {
    this.note = new Note();

    this.noteService.create().subscribe((x) => {
      this.note = x;
    });
    this.additionalNote = new NoteAdditional();

    this.additionalNoteService.create().subscribe((x) => {
      this.additionalNote = x;
    });
  }
}
