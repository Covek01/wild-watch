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
    imageUrl:string

    constructor(
        id:string,
        sightingTime:Date,
        location:LocationI,
        sighter:UserSummary,
        imageUrl:string
    ){
        this.id=id;
        this.sightingTime=sightingTime;
        this.location=location;
        this.sighter=sighter;
        this.imageUrl=imageUrl;
    }
}

export class SightingSummaryHabitat{
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

// public class SightingDto
// {
//     public DateTime SightingTime { get; set; }
//     public GeoJson2DGeographicCoordinates Location { get; set; } = null!;
//     public string? PhotoUrl { get; set; }

//     public string SighterId { get; set; } = null!;

//     public Klasa SpeciesClass { get; set; }
//     public string CommonName { get; set; } = null!;

//     public string ScientificName { get; set; } = null!;

//     public string? ImageUrl { get; set; }
//     public string Description { get; set; } = null!;

//     public string ConservationStatus { get; set; }= null!;

//     public string? Comment { get; set; }

// }
export class GeoJson2DGeographicCoordinates{
    longitude: number
    latitude: number

    constructor(lat: number, long: number){
        this.longitude = long
        this.latitude = lat
    }
}

export class SightingDto {
    sightingTime: Date
    location: GeoJson2DGeographicCoordinates
    photoUrl: string
    sighterId: string
    speciesClass: number
    commonName: string
    scientificName: string
    imageUrl: string
    description: string
    conservationStatus: string
    comment: string

    constructor(
        sightingTime: Date,
        location: GeoJson2DGeographicCoordinates,
        photoUrl: string,
        sighterId: string,
        speciesClass: number,
        commonName: string,
        scientificName: string,
        imageUrl: string,
        description: string,
        conservationStatus: string,
        comment: string
    ) {
        this.sightingTime = sightingTime;
        this.location = location;
        this.photoUrl = photoUrl;
        this.sighterId = sighterId;
        this.speciesClass = speciesClass;
        this.commonName = commonName;
        this.scientificName = scientificName;
        this.imageUrl = imageUrl;
        this.description = description;
        this.conservationStatus = conservationStatus;
        this.comment = comment;
    }
}
