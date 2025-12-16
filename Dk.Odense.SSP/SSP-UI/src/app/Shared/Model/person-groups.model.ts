import { Grouping } from '@domain-models/grouping.model';

export class PersonGroups {
  name!: string;
  cpr!: string;
  groups!: Grouping[];
  person_Id!: string;
  classification_Id?: string;
}
