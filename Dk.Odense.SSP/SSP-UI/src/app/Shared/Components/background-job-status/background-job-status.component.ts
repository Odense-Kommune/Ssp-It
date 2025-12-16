import { Component, OnInit } from '@angular/core';
import { ProcessingJobs } from '@models/processing-jobs.model';
import { ProcessingJobService } from '@services/processing-jobs.service';

@Component({
  selector: 'app-background-job-status',
  templateUrl: './background-job-status.component.html',
  styleUrls: ['./background-job-status.component.scss'],
})
export class BackgroundJobStatusComponent implements OnInit {
  jobProcessing = new Array<ProcessingJobs>();

  constructor(private processingJobsService: ProcessingJobService) {}

  ngOnInit() {
    this.getProcessingJobs();
    setInterval(() => {
      this.getProcessingJobs();
    }, 30000);
  }

  getProcessingJobs() {
    this.processingJobsService.GetProcessingJobs().subscribe((x) => {
      this.jobProcessing = x;
    });
  }
}
