import { Reporter } from '@domain-models/reporter.model';
import { ReportedPerson } from '@domain-models/reported-person.model';
import { Concern } from '@domain-models/concern.model';
import { Assessment } from '@domain-models/assessment.model';
import { Dictionary } from '@models/dictionary.model';

export class VerifyWorry {
  socialSecNum?: string;
  reporter?: Reporter;
  reportedPerson?: ReportedPerson;
  concern?: Concern;
  assessment?: Assessment;
  policeWorryCategory!: Dictionary;
  policeWorryRole!: Dictionary;
}
