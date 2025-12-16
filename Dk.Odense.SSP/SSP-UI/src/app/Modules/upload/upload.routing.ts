import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NeedAuthGuard } from '@auth/auth.guard';
import { UploadIndexComponent } from './pages/upload-index/upload-index.component';

const routes: Routes = [
  {
    path: 'upload',
    component: UploadIndexComponent,
    canActivate: [NeedAuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CsvUploadRoutingModule {}
