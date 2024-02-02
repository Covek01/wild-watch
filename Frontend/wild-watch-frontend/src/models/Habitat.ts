import { SightingSummaryHabitat } from "./Sighting";

export class HabitatSummary {
    id: string

    constructor(
        id: string
    ) {
        this.id = id;
    }
}

export class Habitat {
    id: string
    sightings: SightingSummaryHabitat[]

    constructor(
        id: string,
        sightings: SightingSummaryHabitat[]
    ) {
        this.id = id;
        this.sightings = sightings
    }
}