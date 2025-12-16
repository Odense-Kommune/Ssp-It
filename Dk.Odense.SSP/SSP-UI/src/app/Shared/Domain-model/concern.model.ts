import { Answer } from '@models/answer.enum';

export class Concern {
  crimeConcern!: string;
  reportedToPolice!: Answer;
  notifyConcern!: Answer;
}
