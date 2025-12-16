import { Worry } from '@domain-models/worry.model';
import { ReportedPerson } from '@domain-models/reported-person.model';
import { Concern } from '@domain-models/concern.model';

export class ExcelUploadResult {
  worry!: Worry;
  reportedPerson!: ReportedPerson;
  concern!: Concern;
  errors!: string[];
}
