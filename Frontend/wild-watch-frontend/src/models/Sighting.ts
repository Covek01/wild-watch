import { Point } from "geojson"
import {SpeciesSummary} from "./Species"
import { UserSummary } from "./User"


export class SightingSummaryUser{
    id:string
    sightingTime:Date
    location:Point
    species:SpeciesSummary

    constructor(
        id:string,
        sightingTime:Date,
        location:Point,
        species:SpeciesSummary
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.species=species;
    }
}

export class SightingSummarySpecies{
    id:string
    sightingTime:Date
    location:Point
    sighter:UserSummary

    constructor(
        id:string,
        sightingTime:Date,
        location:Point,
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
    location:Point
    sighter:UserSummary
    species:SpeciesSummary

    constructor(
        id:string,
        sightingTime:Date,
        location:Point,
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
    location:Point
    sighter:UserSummary
    species:SpeciesSummary
    imageUrl:string
    constructor(
        id:string,
        sightingTime:Date,
        location:Point,
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