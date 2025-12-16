import { Grouping } from '@domain-models/grouping.model';
import { Dictionary } from '@models/dictionary.model';
import { SchoolData } from '@models/school-data.model';
import { Categorization } from '@models/categorization.model';

export class Person {
  id!: string;
  name!: string;
  socialSecNum!: string;
  socialWorker?: string;
  age!: string;
  address!: string;
  groupings!: Grouping[];
  worriesCount!: string;
  robustnessesCount!: string;
  sspArea!: Dictionary;
  categorization!: Categorization;
  agendaCategorization!: Categorization;
  sspStopDate!: Date | null;
  schoolData?: SchoolData;
  worryIncrements!: number[];
}
