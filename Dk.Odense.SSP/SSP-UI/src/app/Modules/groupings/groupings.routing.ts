import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GroupingsIndexComponent } from './pages/groupings-index/groupings-index.component';
import { NeedAuthGuard } from '@auth/auth.guard';
import { GroupingType } from '@models/grouping-type';

const routes: Routes = [
  {
    path: 'grouping',
    component: GroupingsIndexComponent,
    canActivate: [NeedAuthGuard],
    data: { type: GroupingType.grouping },
  },

  {
    path: 'grouping/:id',
    component: GroupingsIndexComponent,
    canActivate: [NeedAuthGuard],
    data: { type: GroupingType.grouping },
  },
  {
    path: 'psu',
    component: GroupingsIndexComponent,
    canActivate: [NeedAuthGuard],
    data: { type: GroupingType.psu },
  },
  {
    path: 'psu/:id',
    component: GroupingsIndexComponent,
    canActivate: [NeedAuthGuard],
    data: { type: GroupingType.psu },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupingsRoutingModule {}
