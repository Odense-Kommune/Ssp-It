import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgendaSpecificComponent } from './pages/agenda-specific/agenda-specific.component';
import { AgendaIndexComponent } from './pages/agenda-index/agenda-index.component';
import { NeedAuthGuard } from '@auth/auth.guard';

const routes: Routes = [
  {
    path: 'agenda',
    component: AgendaIndexComponent,
    canActivate: [NeedAuthGuard],
  },
  {
    path: 'agendaspecific',
    component: AgendaSpecificComponent,
    canActivate: [NeedAuthGuard],
  },
  {
    path: 'agendaspecific/:id',
    component: AgendaSpecificComponent,
    canActivate: [NeedAuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AgendaSpecificRoutingModule {}
