import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AgendaSpecificRoutingModule } from './agenda.routing';

import { AgendaIndexComponent } from './pages/agenda-index/agenda-index.component';
import { AgendaSpecificComponent } from './pages/agenda-specific/agenda-specific.component';
import { ActionsComponent } from './components/actions/actions.component';
import { AgendaCurrentListComponent } from './components/agenda-current-list/agenda-current-list.component';
import { AgendaHistoryMenuComponent } from './components/agenda-history-menu/agenda-history-menu.component';
import { AgendaSpecificMenuComponent } from './components/agenda-specific-menu/agenda-specific-menu.component';
import { CreateAgendaComponent } from './components/create-agenda/create-agenda.component';
import { MenuComponent } from './components/menu/menu.component';
import { PendingWorryComponent } from './components/pending-worry/pending-worry.component';
import { RobustnessComponent } from './components/robustness/robustness.component';
import { WorryActionsComponent } from './components/worry-actions/worry-actions.component';

import { AgendaSpecificMenuCrimesceneComponent } from './components/agenda-specific-menu-crimescene/agenda-specific-menu-crimescene.component';

// MDB Angular Pro
import { MdbDatepickerModule } from 'mdb-angular-ui-kit/datepicker';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';
import { MdbFormsModule } from 'mdb-angular-ui-kit/forms';

@NgModule({
  imports: [
    SharedModule,
    FormsModule,
    CommonModule,
    AgendaSpecificRoutingModule,
    MdbDatepickerModule,
    MdbRippleModule,
    MdbFormsModule,
  ],
  exports: [],
  declarations: [
    AgendaIndexComponent,
    AgendaSpecificComponent,
    ActionsComponent,
    AgendaCurrentListComponent,
    AgendaHistoryMenuComponent,
    AgendaSpecificMenuComponent,
    CreateAgendaComponent,
    MenuComponent,
    PendingWorryComponent,
    RobustnessComponent,
    WorryActionsComponent,
    AgendaSpecificMenuCrimesceneComponent,
  ],
  providers: [],
})
export class AgendaModule {}
