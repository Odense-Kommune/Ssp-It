import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MdbDatepickerModule } from 'mdb-angular-ui-kit/datepicker';
import { MdbAutocompleteModule } from 'mdb-angular-ui-kit/autocomplete';
import { MdbFormsModule } from 'mdb-angular-ui-kit/forms';

// Import Services
import { NoteAdditionalService } from '@services/note-additional.service';
import { AgendaItemService } from '@services/agenda-item.service';
import { AgendaOverviewService } from '@services/agenda-overview.service';
import { AgendaSpecificService } from '@services/agenda-specific.service';
import { AgendaService } from '@services/agenda.service';
import { AreaRuleService } from '@services/area-rule.service';
import { BaseService } from '@services/base.service';
import { CategorizationService } from '@services/categorization.service';
import { UploadService } from '@services/upload.service';
import { ExportService } from '@services/export.service';
import { GroupingService } from '@services/grouping.service';
import { ModalService } from '@services/modal.service';
import { NoteService } from '@services/note.service';
import { PersonService } from '@services/person.service';
import { PoliceWorryCategoryService } from '@services/police-worry-category.service';
import { PoliceWorryRoleService } from '@services/police-worry-role.service';
import { ProcessingJobService } from '@services/processing-jobs.service';
import { RobustnessService } from '@services/robustness.service';
import { SchoolDataService } from '@services/school-data.service';
import { SourceService } from '@services/source.service';
import { SpecificPersonService } from '@services/specific-person.service';
import { SspAreaService } from '@services/ssp-area.service';
import { ToastService } from '@services/toast.service';
import { VerifyWorryService } from '@services/verify-worry.service';
import { VerifyWorryMenuItemService } from '@services/verify-worry-menu-item.service';
import { WorryService } from '@services/worry.service';

// Import Pipes
import { DynamicFilter } from './pipes/dynamic-filter.pipe';
import { MondayByWeekPipe } from './pipes/monday-by-week.pipe';
import { SortBy } from './pipes/sort-by.pipe';
import { SortByDesc } from './pipes/sort-by-desc.pipe';
import { SortVerifyWorryMenuItem } from './pipes/sort-by-menu.pipe';
import { SspStopPipe } from './pipes/ssp-stop.pipe';

// Import components
import { AssessmentComponent } from '@components/assessment/assessment.component';
import { BackgroundJobStatusComponent } from '@components/background-job-status/background-job-status.component';
import { CollapseComponent } from '@components/collapse/collapse.component';
import { CollapseMenuComponent } from '@components/collapse-menu/collapse-menu.component';
import { ConcernComponent } from '@components/concern/concern.component';
import { DatacardComponent } from '@components/datacard/datacard.component';
import { DatePickerComponent } from '@components/date-picker/date-picker.component';
import { NotesComponent } from '@components/notes/notes.component';
import { PageRobustnessComponent } from '@components/page-robustness/page-robustness.component';
import { ReporterComponent } from '@components/reporter/reporter.component';
import { StatisticsComponent } from '@components/statistics/statistics.component';
import { ToastComponent } from '@components/toast/toast.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// Declare services
const services = [
  NoteAdditionalService,
  AgendaItemService,
  AgendaOverviewService,
  AgendaSpecificService,
  AgendaService,
  AreaRuleService,
  BaseService,
  CategorizationService,
  UploadService,
  ExportService,
  GroupingService,
  ModalService,
  NoteService,
  PersonService,
  PoliceWorryCategoryService,
  PoliceWorryRoleService,
  ProcessingJobService,
  RobustnessService,
  SchoolDataService,
  SourceService,
  SpecificPersonService,
  SspAreaService,
  ToastService,
  VerifyWorryService,
  VerifyWorryMenuItemService,
  WorryService,
];

// Declare components
const components = [
  AssessmentComponent,
  BackgroundJobStatusComponent,
  CollapseComponent,
  CollapseMenuComponent,
  ConcernComponent,
  DatacardComponent,
  DatePickerComponent,
  NotesComponent,
  PageRobustnessComponent,
  ReporterComponent,
  StatisticsComponent,
  ToastComponent,
];

// Declare pipes
const pipes = [
  DynamicFilter,
  MondayByWeekPipe,
  SortBy,
  SortByDesc,
  SortVerifyWorryMenuItem,
  SspStopPipe,
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    RouterModule,
    MdbDatepickerModule,
    MdbAutocompleteModule,
    MdbFormsModule,
  ],
  exports: [...components, ...pipes],
  declarations: [...components, ...pipes],
  providers: [...services],
})
export class SharedModule {}
