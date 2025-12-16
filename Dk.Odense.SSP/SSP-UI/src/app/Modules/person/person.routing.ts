import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NeedAuthGuard } from '@auth/auth.guard';
import { PersonSpecificComponent } from './pages/person-specific/person-specific.component';
import { PersonIndexComponent } from './pages/person-index/person-index.component';

const routes: Routes = [
  {
    path: 'persons',
    component: PersonIndexComponent,
    canActivate: [NeedAuthGuard],
  },
  {
    path: 'person_specific/:id',
    component: PersonSpecificComponent,
    canActivate: [NeedAuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PersonRoutingModule {}
