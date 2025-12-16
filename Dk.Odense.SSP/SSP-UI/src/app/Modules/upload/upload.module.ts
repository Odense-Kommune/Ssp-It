import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '@shared/shared.module';
import { CsvUploadRoutingModule } from './upload.routing';
import { UploadIndexComponent } from './pages/upload-index/upload-index.component';

@NgModule({
  imports: [CommonModule, FormsModule, SharedModule, CsvUploadRoutingModule],
  exports: [],
  declarations: [UploadIndexComponent],
  providers: [],
})
export class CsvUploadModule {}
