import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Person } from '@models/person.model';
import { ExportService } from '@services/export.service';

@Component({
  selector: 'app-collapse-menu',
  templateUrl: './collapse-menu.component.html',
  styleUrls: ['./collapse-menu.component.scss'],
})
export class CollapseMenuComponent implements OnInit {
  @Input() person!: Person;
  @Input() collapsed!: boolean;
  @Input() agendaItemId!: string;
  @Input() agendaNumber!: number;

  @ViewChild('inputRef', { static: true }) input!: ElementRef;
  isCollapsed!: boolean;

  constructor(private exportService: ExportService) {}

  ngOnInit() {
    if (this.collapsed) {
      this.isCollapsed = true;
    } else {
      this.isCollapsed = false;
    }
  }

  collapse() {
    this.isCollapsed = !this.isCollapsed;
  }

  downloadItem() {
    return this.exportService
      .exportAgendaItem(this.agendaItemId)
      .subscribe((fileData) => {
        const blob = new Blob([fileData], {
          type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
        });

        const link = document.createElement('a');

        if (link.download !== undefined) {
          const url = URL.createObjectURL(blob);
          link.setAttribute('href', url);
          link.setAttribute(
            'download',
            `DagsordenPunkt Uge ${this.agendaNumber?.toString()} - ${
              this.person.name
            }`
          );
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }
      });
  }
}
