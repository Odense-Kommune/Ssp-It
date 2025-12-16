export enum Answer {
    Yes = "Ja",
    No = "Nej",
    DontKnow = "Det ved jeg ikke"
}

export class MapAnswer {
    map(value: number) {
        switch (value) {
            case 0:
                return Answer.Yes
            case 1:
                return Answer.No
            default:
                return Answer.DontKnow
        }
    }
}
