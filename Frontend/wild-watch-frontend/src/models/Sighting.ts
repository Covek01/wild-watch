import {SpeciesSummary} from "./Species"
import { UserSummary } from "./User"
import { LocationI } from "../types/location"


export class SightingSummaryUser{
    id:string
    sightingTime:Date
    location:LocationI
    species:SpeciesSummary
    imageUrl:string
    constructor(
        id:string,
        sightingTime:Date,
        location:LocationI,
        species:SpeciesSummary,
        imageUrl:string
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.species=species;
        this.imageUrl=imageUrl;
    }
}

export class SightingSummarySpecies{
    id:string
    sightingTime:Date
    location:LocationI
    sighter:UserSummary

    constructor(
        id:string,
        sightingTime:Date,
        location:LocationI,
        sighter:UserSummary
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.sighter=sighter;
    }
}

export class SightingSummaryHabitat{
    id:string
    sightingTime:Date
    location:LocationI
    sighter:UserSummary
    species:SpeciesSummary

    constructor(
        id:string,
        sightingTime:Date,
        location:LocationI,
        sighter:UserSummary,
        species:SpeciesSummary
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.sighter=sighter;
        this.species=species
    }
}

export class Sighting{
    id:string
    sightingTime:Date
    location:LocationI
    sighter:UserSummary
    species:SpeciesSummary
    imageUrl:string
    constructor(
        id:string,
        sightingTime:Date,
        location:LocationI,
        sighter:UserSummary,
        species:SpeciesSummary,
        imageUrl:string
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.sighter=sighter;
        this.species=species;
        this.imageUrl=imageUrl;
    }
}