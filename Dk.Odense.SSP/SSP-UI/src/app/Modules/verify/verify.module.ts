import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@shared/shared.module';
import { VerifyIndexComponent } from './pages/verify-index/verify-index.component';
import { VerifyRoutingModule } from './verify.routing';
import { ReportingOfComponent } from './components/reporting-of/reporting-of.component';
import { WorryMenuComponent } from './components/worry-menu/worry-menu.component';
import { PendingStatusComponent } from './components/pending-status/pending-status.component';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';

@NgModule({
  imports: [CommonModule, VerifyRoutingModule, SharedModule, MdbRippleModule],
  declarations: [
    ReportingOfComponent,
    WorryMenuComponent,
    PendingStatusComponent,
    VerifyIndexComponent,
  ],
})
export class VerifyModule {}
