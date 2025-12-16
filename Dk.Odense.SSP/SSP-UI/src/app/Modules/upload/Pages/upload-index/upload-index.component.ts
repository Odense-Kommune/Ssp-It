import { Component, OnInit } from '@angular/core';
import { Source } from '@domain-models/Source.model';
import { UploadService } from '@services/upload.service';
import { SourceService } from '@services/source.service';
import { ExcelUploadResult } from '@models/excel-upload-result.model';
import { ReturnStatement } from '@angular/compiler';

@Component({
  selector: 'app-upload-index',
  templateUrl: './upload-index.component.html',
  styleUrls: ['./upload-index.component.scss'],
})
export class UploadIndexComponent implements OnInit {
  fileToUpload?: File;
  source: string;
  step: number;
  data = new Array<ExcelUploadResult>();
  errorResponse: boolean;
  isLoading: boolean;
  sources = new Array<Source>();

  constructor(
    private uploadService: UploadService,
    private sourceService: SourceService
  ) {
    this.source = '';
    this.step = 0;
    this.errorResponse = false;
    this.isLoading = false;
    this.sourceService.getFacetList().subscribe((r) => {
      this.sources = r;
    });
  }

  ngOnInit() {}

  DownloadSkab() {
    const file = document.createElement('a');
    file.setAttribute('type', 'hidden');
    file.href = 'assets/excel/skab.xlsx';
    file.download = 'skab.xlsx';
    document.body.appendChild(file);
    file.click();
    file.remove();
  }

  advanceStep() {
    const stepper = document.getElementsByClassName('stepper')[0];
    let curStep = stepper.querySelector('[data-active]');
    const nextStep = curStep?.nextElementSibling;
    if (!nextStep) {
      return;
    }
    curStep?.removeAttribute('data-active');
    curStep?.setAttribute('data-ok', '');
    nextStep.setAttribute('data-active', '');
    this.step++;
  }

  reduceStep() {
    const stepper = document.getElementsByClassName('stepper')[0];
    const curStep = stepper.querySelector('[data-active]');
    const nextStep = curStep?.previousElementSibling;
    if (!nextStep) {
      return;
    }
    curStep.removeAttribute('data-active');
    nextStep.removeAttribute('data-ok');
    nextStep.setAttribute('data-active', '');
    this.step--;
    this.errorResponse = false;
  }

  handleFileInput(event: Event): void {
    let fileList = (event.target as HTMLInputElement).files;

    if (!fileList) return;

    this.fileToUpload = fileList[0];
    let input = document.getElementById('filepicker_text');

    if (!input) return;

    input.innerHTML = ' ' + fileList[0].name;
  }

  submitForm(): void {
    if (this.fileToUpload === undefined || this.source === '') {
      return;
    }
    this.isLoading = true;
    this.uploadService.processExcel(this.fileToUpload).subscribe(
      (data: ExcelUploadResult[]) => {
        this.advanceStep();
        this.data = new Array<ExcelUploadResult>();
        this.data = data;
        this.isLoading = false;
      },
      (error) => {}
    );
  }

  submitProcessedData() {
    this.isLoading = true;
    this.uploadService
      .uploadExcelData(this.source, this.data)
      .subscribe((response) => {
        this.advanceStep();
        this.isLoading = false;
      });
  }
}
