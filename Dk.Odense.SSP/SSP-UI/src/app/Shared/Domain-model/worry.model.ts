import { Concern } from '@domain-models/concern.model';

export class Worry {
  SspArea!: string;
  crimeScene!: string;
  createdDate!: Date;
  processed?: Date;
  groundless!: boolean;
  approved!: boolean;
  increment!: number;
  concern_Id!: string;
  concern!: Concern | null;
  person_Id!: string;
  reportedPerson_Id!: string;
  reporter_Id!: string;
  assessment_Id!: string;
  policeWorryCategory_Id!: string;
  policeWorryRole_Id!: string;
  source_Id!: string;
}
