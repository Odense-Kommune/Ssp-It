import { Component, OnInit, OnChanges } from '@angular/core';
import { Agenda } from '@models/agenda.model';
import { Dictionary } from '@models/dictionary.model';
import { AgendaSpecificMenuItem } from '@models/agenda-specific-menu-item.model';
import { VerifyWorry } from '@models/verify-worry.model';
import { Note } from '@domain-models/note.model';
import { Person } from '@models/person.model';
import { PoliceWorryCategoryService } from '@services/police-worry-category.service';
import { PoliceWorryRoleService } from '@services/police-worry-role.service';
import { VerifyWorryService } from '@services/verify-worry.service';
import { NoteService } from '@services/note.service';
import { AgendaSpecificService } from '@services/agenda-specific.service';
import { WorryService } from '@services/worry.service';
import { SspAreaService } from '@services/ssp-area.service';
import { CategorizationService } from '@services/categorization.service';
import { PersonService } from '@services/person.service';
import { ActivatedRoute } from '@angular/router';
import { AgendaService } from '@services/agenda.service';
import { AgendaItemService } from '@services/agenda-item.service';
import { Grouping } from '@domain-models/grouping.model';
import { GroupingService } from '@services/grouping.service';
import { ExportService } from '@services/export.service';
import { WorryItem } from '@models/worry-item.model';
import { Categorization } from '@models/categorization.model';
import { NoteAdditional } from '@domain-models/note-additional.model';
import { NoteAdditionalService } from '@services/note-additional.service';

@Component({
  selector: 'app-agenda-specific',
  templateUrl: './agenda-specific.component.html',
  styleUrls: ['./agenda-specific.component.scss'],
})
export class AgendaSpecificComponent implements OnInit, OnChanges {
  selectedMenuItem!: string;
  agendaId!: string;
  agenda = new Agenda();
  agendaItemId!: string;
  archived?: boolean;
  isLoading = false;
  scrollNumber?: number;

  donwloadPath?: string;
  policeWorryCategories?: Dictionary[];
  policeWorryRoles?: Dictionary[];
  sspAreas = new Array<Dictionary>();
  categorizations!: Categorization[];
  menuItems: AgendaSpecificMenuItem[];
  worry = new VerifyWorry();
  notes = new Array<Note>();
  note = new Note();
  additionalNotes = new Array<NoteAdditional>();
  additionalNote = new NoteAdditional();
  person = new Person();
  groups: Grouping[] = [];
  showCrimeSceneMenu = false;
  choosenCrimeScene!: string;

  constructor(
    private policeWorryCategoryService: PoliceWorryCategoryService,
    private policeWorryRoleService: PoliceWorryRoleService,
    private verifyWorryService: VerifyWorryService,
    private noteService: NoteService,
    private agendaSpecificService: AgendaSpecificService,
    private worryService: WorryService,
    private sspAreaService: SspAreaService,
    private categorizationService: CategorizationService,
    private personService: PersonService,
    private route: ActivatedRoute,
    private agendaService: AgendaService,
    private agendaItemService: AgendaItemService,
    private groupingService: GroupingService,
    private exportService: ExportService,
    private additionalNoteService: NoteAdditionalService
  ) {
    this.agendaId = '';
    this.policeWorryCategories = [];
    this.policeWorryRoles = [];
    this.menuItems = [];
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id != undefined || id != null) this.agendaId = id;
    this.policeWorryCategories = new Array<Dictionary>();
    this.getInitialData();
  }

  getPoliceWorryCategories(): void {
    this.policeWorryCategoryService
      .getFacetList()
      .subscribe((x) => (this.policeWorryCategories = x));
  }

  savePoliceWorryCategory(id: string): void {
    this.worry.policeWorryCategory = new Dictionary();
    this.worry.policeWorryCategory.id = id;

    let r = new WorryItem();
    for (let index = 0; index < this.menuItems.length; index++) {
      let temp = this.menuItems[index].worryItems.find(
        (y) => y.id === this.selectedMenuItem
      );

      if (temp) r = temp;
    }
    r.policeCat = id;

    this.worryService
      .setPoliceWorryCategory(this.selectedMenuItem, id)
      .subscribe();
  }

  getPoliceWorryRoles(): void {
    this.policeWorryRoleService
      .getFacetList()
      .subscribe((x) => (this.policeWorryRoles = x));
  }

  savePoliceWorryRole(id: string): void {
    this.worry.policeWorryRole = new Dictionary();
    this.worry.policeWorryRole.id = id;

    let r = new WorryItem();
    for (let index = 0; index < this.menuItems.length; index++) {
      let temp = this.menuItems[index].worryItems.find(
        (y) => y.id === this.selectedMenuItem
      );

      if (temp) r = temp;
    }

    r.policeRole = id;

    this.worryService.setPoliceWorryRole(this.selectedMenuItem, id).subscribe();
  }

  saveSocialWorker(socialWorker: string) {
    this.person.socialWorker = socialWorker;
    this.personService
      .setSocialWorker(this.person.id, this.person.socialWorker)
      .subscribe();
  }

  getSspArea(id: string): string {
    for (const x of this.sspAreas) {
      if (x.id === id && x.value) {
        return x.value;
      }
    }
    return '';
  }

  getSspAreas(): void {
    this.sspAreaService.getFacetList().subscribe((x) => {
      this.sspAreas = x;
    });
  }

  setSspArea(id: string): void {
    this.person.sspArea.id = id;
    this.personService.setSspArea(this.person.id, id).subscribe((x) => {
      this.updateMenuItemDatacard(this.agendaItemId, '', id);
    });
  }

  getCategorization(id: string): string {
    for (const x of this.categorizations) {
      if (x.id === id) {
        return x.value;
      }
    }
    return '';
  }

  getCategorizations(): void {
    this.categorizationService.getFacetList().subscribe((x) => {
      this.categorizations = x;
    });
  }

  setCategorization(id: string): void {
    const r = this.categorizations?.find((x) => x.id == id);

    if (r?.deleteAfterSspEnd === true && this.person?.sspStopDate !== null) {
      alert(
        'Borgeren har en ssp-stopdato tastet i forvejen. Hvis denne er gammel, bør du slette den.'
      );
    }

    this.agendaItemService
      .setCategorization(this.agendaItemId, id)
      .subscribe((x) => {
        this.updateMenuItemDatacard(this.agendaItemId, id, '');
      });
  }

  getVerifyWorry(id: string): void {
    this.verifyWorryService.get(id).subscribe((x) => {
      this.worry = x;
    });
  }

  getMenuItems(id: string): void {
    this.agendaSpecificService.getMenuItems(id).subscribe((x) => {
      this.menuItems = x;
      this.filterCrimesceneMenu(x);
    });
  }

  filterCrimesceneMenu(menuItems: AgendaSpecificMenuItem[]): void {
    let worryItem: WorryItem[];

    menuItems.forEach((x) => (worryItem = [...x.worryItems]));
  }

  getNotes(id: string): void {
    this.noteService.getNotesByPerson(id).subscribe((notes) => {
      this.notes = notes;
    });
    this.additionalNoteService
      .getNotesByPerson(id)
      .subscribe((x) => (this.additionalNotes = x));
  }

  onSaveNote(note: Note): void {
    this.note.value = note.value;
    this.note.person_id = this.person?.id ?? '';
    this.noteService
      .saveNoteToPerson(this.note)
      .subscribe((x) => (this.note = x));
  }
  onSaveAddNote(text: NoteAdditional): void {
    this.additionalNote.value = text.value;
    this.additionalNote.person_id = this.person.id;
    this.additionalNoteService
      .saveNoteToPerson(this.additionalNote)
      .subscribe((x) => (this.additionalNote = x));
  }

  getSelectedMenuData(value: {
    id: string;
    person: Person;
    agendaItemId: string;
  }) {
    if (!this.person) {
      this.getNotes(value.person.id);
      this.resetNote();
    } else if (this.person.id !== value.person.id) {
      this.getNotes(value.person.id);
      this.resetNote();
    }
    this.person = value.person;
    this.selectedMenuItem = value.id;
    this.agendaItemId = value.agendaItemId;
    this.getVerifyWorry(value.id);
  }

  closeCrimeSceneMenu() {
    this.showCrimeSceneMenu = false;
    setTimeout(() => {
      const scroll = document.querySelector('.scrollable');
      scroll?.scrollTo(0, this.scrollNumber ?? 0);
    }, 100);
  }

  chooseCrimeScene(value: string) {
    const scroll = document.querySelector('.scrollable');
    this.scrollNumber = scroll?.scrollTop;
    this.choosenCrimeScene = value;
    this.showCrimeSceneMenu = true;
  }

  getAgenda() {
    this.agendaService.get(this.agendaId).subscribe((x) => {
      this.agenda = x;
      if (this.agenda.meetingHeld) {
        this.archived = true;
      }
    });
  }

  getInitialData() {
    this.getPoliceWorryCategories();
    this.getPoliceWorryRoles();
    this.getSspAreas();
    this.getCategorizations();
    this.getMenuItems(this.agendaId);
    this.getAgenda();
    this.groupingService.list().subscribe((x) => (this.groups = x));
  }

  getDonwloadPath() {
    this.isLoading = true;
    return this.exportService
      .exportAgenda(this.agendaId)
      .subscribe((fileData) => {
        const blob = new Blob([fileData], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        });

        const link = document.createElement('a');

        if (link.download !== undefined) {
          const url = URL.createObjectURL(blob);
          link.setAttribute('href', url);
          link.setAttribute(
            'download',
            `Dagsorden-Exel-Uge:${this.agenda.agendaNumber.toString()}`
          );
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }
        this.isLoading = false;
      });
  }

  refresh(event: boolean) {
    this.getInitialData();
  }

  updateMenuItemDatacard(
    id: string,
    categorizationId: string,
    sspAreaId: string
  ) {
    for (const x of this.menuItems) {
      if (x.id === id) {
        if (categorizationId !== '') {
          x.person.agendaCategorization.value =
            this.getCategorization(categorizationId);
        }
        if (sspAreaId !== '') {
          x.person.sspArea.value = this.getSspArea(sspAreaId);
        }
      }
    }
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

  ngOnChanges() {
    this.getInitialData();
  }
}
