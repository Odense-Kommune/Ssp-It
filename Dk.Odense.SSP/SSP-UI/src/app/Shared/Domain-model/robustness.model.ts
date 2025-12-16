import { ReportedPerson } from '@domain-models/reported-person.model';
import { Assessment } from '@domain-models/assessment.model';
import { Reporter } from '@domain-models/reporter.model';

export class Robustness {
  createdDate!: Date;
  increment!: number;
  enrollmentPlace!: string;
  person_Id!: string;
  reportedPerson_Id!: string;
  reportedPerson!: ReportedPerson;
  reporter_Id!: string;
  reporter!: Reporter;
  assessment_Id!: string;
  assessment!: Assessment;
}
