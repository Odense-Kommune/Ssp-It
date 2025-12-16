import { TrafficLight } from '@models/traffic-light.enum';

export class Assessment {
  socialBehavior!: TrafficLight;
  socialBehaviorElaborate!: string;
  positiveSupport!: TrafficLight;
  positiveSupportElaborate!: string;
  skills!: TrafficLight;
  skillsElaborate!: string;
  drugRelationship!: TrafficLight;
  drugRelationshipElaborate!: string;
  goodFriends!: TrafficLight;
  goodFriendsElaborate!: string;
  futureDreams!: TrafficLight;
  futureDreamsElaborate!: string;
}
