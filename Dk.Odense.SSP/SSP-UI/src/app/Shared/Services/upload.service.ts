import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { Worry } from '@domain-models/worry.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExcelUploadResult } from '@models/excel-upload-result.model';

@Injectable({
  providedIn: 'root',
})
export class UploadService extends BaseService<Worry> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'upload';
  }
  processExcel(file: File): Observable<ExcelUploadResult[]> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.getHTTPService().post<ExcelUploadResult[]>(
      this.baseUrl + this.endpoint + '/UploadExcel',
      formData,
      {}
    );
  }
  uploadExcelData(sourceID: string, worries: ExcelUploadResult[]) {
    // adding source ID:
    for (const csv of worries) {
      csv.worry.source_Id = sourceID;
    }
    return this.postToEndpoint<ExcelUploadResult[]>(
      worries,
      'ExcelDataToWorries'
    );
  }
}
