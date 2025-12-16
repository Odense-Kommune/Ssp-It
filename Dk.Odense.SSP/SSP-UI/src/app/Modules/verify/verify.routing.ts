import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VerifyIndexComponent } from './pages/verify-index/verify-index.component';
import { NeedAuthGuard } from '@auth/auth.guard';

const routes: Routes = [
  {
    path: 'verify',
    component: VerifyIndexComponent,
    canActivate: [NeedAuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VerifyRoutingModule {}
