import { Component, OnInit } from '@angular/core';
import { PersonGrouping } from '@domain-models/person-grouping.model';
import { Categorization } from '@models/categorization.model';
import { Person } from '@models/person.model';
import { CategorizationService } from '@services/categorization.service';
import { ExportService } from '@services/export.service';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';

@Component({
  selector: 'app-grouping-modal',
  templateUrl: './grouping-modal.component.html',
  styleUrls: ['./grouping-modal.component.scss'],
})
export class GroupingModalComponent implements OnInit {
  persons: PersonGrouping[] | null = null;
  categorizations: Categorization[] | null = null;
  groupId: string | null = null;
  groupingsType: string | null = null;
  downloading = false;
  error = false;

  selectedPersons: string[] = [];
  selectedCategorizations: string[] = [];

  constructor(
    public modalRef: MdbModalRef<GroupingModalComponent>,
    public categorizationService: CategorizationService,
    public exportService: ExportService
  ) {}

  ngOnInit(): void {
    this.getCategorizations();
    console.log(this.persons);
    this.selectedPersons = this.persons?.map((p) => p.person_Id) ?? [];
    console.log(this.selectedPersons);
  }

  close(): void {
    this.modalRef.close();
  }

  getCategorizations() {
    this.categorizationService.getFacetList().subscribe((r) => {
      this.categorizations = r;
      this.selectedCategorizations = r?.map((c) => c.id) ?? [];
    });
  }

  onPersonChange($event: any) {
    if ($event.target.checked) {
      this.selectedPersons.push($event.target.value);
    } else {
      this.selectedPersons.splice(
        this.selectedPersons.indexOf($event.target.value),
        1
      );
    }
  }

  onCategoryChange($event: any) {
    if ($event.target.checked) {
      this.selectedCategorizations.push($event.target.value);
    } else {
      this.selectedCategorizations.splice(
        this.selectedCategorizations.indexOf($event.target.value),
        1
      );
    }
  }

  getExport() {
    this.downloading = true;
    if (
      this.selectedPersons.length === 0 &&
      this.selectedCategorizations.length === 0
    )
      return;

    this.exportService
      .exportCrossRef(
        this.groupId,
        this.selectedPersons,
        this.selectedCategorizations,
        this.groupingsType
      )
      .subscribe({
        next: (n) => {
          const blob = new Blob([n], {
            type: n.type,
          });
          const link = document.createElement('a');
          if (link.download !== undefined) {
            const url = URL.createObjectURL(blob);
            link.setAttribute('href', url);
            link.setAttribute('download', `Cross Reference`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
          }
        },
        error: (e) => {
          this.error = true;
        },
        complete: () => {
          this.downloading = false;
          console.log(this.downloading);
        },
      });
  }
}
