import { ReportedPerson } from '@domain-models/reported-person.model';
import { SocialSecData } from '@models/SocialSecData.model';

export class VerifyWorryMenuItem {
  id!: string;
  increment!: string;
  source!: string;
  reportedPerson!: ReportedPerson;
  approved!: boolean;
  groundless!: boolean;
  socialSecData!: SocialSecData;
}
