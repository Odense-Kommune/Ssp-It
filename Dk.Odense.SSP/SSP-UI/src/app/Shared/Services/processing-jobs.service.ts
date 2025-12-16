import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { ProcessingJobs } from '@models/processing-jobs.model';

@Injectable({
  providedIn: 'root',
})
export class ProcessingJobService extends BaseService<any> {
  constructor(private httpClient: HttpClient) {
    super(httpClient);
    this.endpoint = 'hangfirestatus';
  }

  GetProcessingJobs() {
    return this.getHTTPService().get<ProcessingJobs[]>(
      this.baseUrl + this.endpoint + '/processingjobs'
    );
  }
}
