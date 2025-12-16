import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { FormsModule } from '@angular/forms';
import { GroupingsRoutingModule } from './groupings.routing';
import { GroupingsIndexComponent } from './pages/groupings-index/groupings-index.component';
import { AddPersonComponent } from './components/add-person/add-person.component';
import { FilterComponent } from './components/filter/filter.component';
import { PeopleComponent } from './components/people/people.component';
import { SinglePersonComponent } from './components/single-person/single-person.component';

import { MdbFormsModule } from 'mdb-angular-ui-kit/forms';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';
import { MdbModalModule } from 'mdb-angular-ui-kit/modal';
import { GroupingStatsComponent } from './components/grouping-stats/grouping-stats.component';
import { GroupingModalComponent } from './components/grouping-modal/grouping-modal.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    GroupingsRoutingModule,
    MdbFormsModule,
    MdbRippleModule,
    MdbModalModule,
  ],
  exports: [],
  declarations: [
    GroupingsIndexComponent,
    AddPersonComponent,
    FilterComponent,
    PeopleComponent,
    SinglePersonComponent,
    GroupingStatsComponent,
    GroupingModalComponent,
  ],
  providers: [],
})
export class GroupingsModule {}
