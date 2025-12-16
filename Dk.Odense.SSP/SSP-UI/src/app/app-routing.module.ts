import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NeedAuthGuard } from '@auth/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [NeedAuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
