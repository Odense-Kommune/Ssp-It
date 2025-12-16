import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@shared/shared.module';
import { FormsModule } from '@angular/forms';
import { PersonIndexComponent } from './pages/person-index/person-index.component';
import { PersonSpecificComponent } from './pages/person-specific/person-specific.component';
import { MenuComponent } from './components/menu/menu.component';
import { PersonRoutingModule } from './person.routing';

@NgModule({
  imports: [CommonModule, SharedModule, FormsModule, PersonRoutingModule],
  declarations: [PersonIndexComponent, PersonSpecificComponent, MenuComponent],
})
export class PersonModule {}
