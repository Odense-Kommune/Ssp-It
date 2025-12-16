export enum TrafficLight {
    Red = "Rød",
    Yellow = "Gul",
    Green = "Grøn"
}

export class MapTrafficLight {
    map(value: number) {
        switch (value) {
            case 0:
                return TrafficLight.Red
            case 1:
                return TrafficLight.Yellow
            case 2:
                return TrafficLight.Green
            default:
                return TrafficLight.Red
        }
    }
}
