import { SightingSummaryUser } from "./Sighting"
import { LocationI } from "../types/location"
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
    location:LocationI
    sightings:SightingSummaryUser[]
    favouriteSpecies:string[]

    constructor(
        id:string,
        name:string,
        email:string,
        imageUrl:string,
        dateOfBrith:Date,
        location:LocationI,
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
