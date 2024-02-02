import { Point } from "geojson"
import { SightingSummaryUser } from "./Sighting"
export class UserSummary{
    id:string
    name:string
    email:string
    numberOfSightings:number

    constructor(
        id:string,
        name:string,
        email:string,
        numberOfSightings:number
    ){
        this.id=id;
        this.name=name;
        this.email=email;
        this.numberOfSightings=numberOfSightings;
    }
}

export class User{
    id:string
    name:string
    email:string
    imageUrl:string
    dateOfBirth:Date
    location:Point
    sightings:SightingSummaryUser[]
    favouriteSpecies:string[]

    constructor(
        id:string,
        name:string,
        email:string,
        imageUrl:string,
        dateOfBrith:Date,
        location:Point,
        sightings:SightingSummaryUser[],
        favouriteSpecies:string[]
    ){
        this.id=id;
        this.name=name;
        this.email=email;
        this.imageUrl=imageUrl;
        this.dateOfBirth=dateOfBrith;
        this.location=location;
        this.sightings=sightings;
        this.favouriteSpecies=favouriteSpecies;
    }
}
