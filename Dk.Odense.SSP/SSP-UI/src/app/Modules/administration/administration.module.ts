import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@shared/shared.module';
import { AdministrationRoutingModule } from './administration.routing';

// Import components
import { AdministrationIndexComponent } from './pages/administration-index/administration-index.component';
import { AutomationRulesComponent } from './components/automation-rules/automation-rules.component';
import { CategorizationEditorComponent } from './components/categorization-editor/categorization-editor.component';
import { CategorizationListItemComponent } from './components/categorization-list-item/categorization-list-item.component';
import { FasetEditorComponent } from './components/faset-editor/faset-editor.component';
import { FasetListItemComponent } from './components/faset-list-item/faset-list-item.component';
import { RuleItemComponent } from './components/rule-item/rule-item.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    SharedModule,
    FormsModule,
  ],
  declarations: [
    AdministrationIndexComponent,
    AutomationRulesComponent,
    CategorizationEditorComponent,
    CategorizationListItemComponent,
    FasetEditorComponent,
    FasetListItemComponent,
    RuleItemComponent,
  ],
})
export class AdministrationModule {}
