import React, { useEffect, useRef, useState } from 'react'
import Bar from '../crucials/Bar'
import { Circle, MapContainer, Marker, Popup, TileLayer, useMap } from 'react-leaflet'
import { useTheme } from '@emotion/react'
import { useMapContext } from '../../contexts/map.context'
import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';
import L, { divIcon } from 'leaflet'
import { Habitat } from '../../models/Habitat'
import HabitatService from '../../services/HabitatService'
import { useSnackbar } from '../../contexts/snackbar.context'
import { Sighting } from '../../models/Sighting'
import { UserSummary } from '../../models/User'
import { Klasa, SpeciesSummary } from '../../models/Species'
import { Point } from "geojson"
import { renderToStaticMarkup } from 'react-dom/server';
import SightingMarker from './sightingsMarker'
import { LocationI } from '../../types/location'
import { green } from '@mui/material/colors'
import HabitatCircle from './habitatCircle'
import MarkerClusterGroup from 'react-leaflet-cluster'
let DefaultIcon = L.icon({
    iconUrl: icon,
    shadowUrl: iconShadow
});




L.Marker.prototype.options.icon = DefaultIcon
const Map: React.FC = () => {
    const [habitatsState, setHabitatsState] = useState<Habitat[]>([]);
    const [habitatsCenterState, setHabitatsCenterState] = useState<LocationI[]>([]);
    const [habitatsRadiusState, setHabitatsRadiusState] = useState<number[]>([]);
    const [sightingsState, setSightingsState] = useState<Sighting[]>([]);
    const [sightingMarkersState, setSightingMarkersState] = useState<L.DivIcon[]>([])
    const { setSightings, setHabitats, sightings, habitats } = useMapContext();
    const snackBar = useSnackbar();
    let defaultCenter = { lat: 45, lng: 20.5 };
    let defaultZoom = 13;

    useEffect(() => {
        if (habitats.length === 0 && sightings.length === 0) {
            HabitatService.GetHabitatsWithNumberOfSightings()
                .then(res => {
                    setHabitatsState(res.data ?? [])
                })
                .catch(err => {
                    snackBar.openSnackbar({
                        message: err,
                        severity: 'error',
                    })
                })
        }
        else {
            setHabitatsState(habitats)
            setSightingsState(sightings)
        }
        // const sig1: Sighting = new Sighting(
        //     "testid",
        //     new Date(Date.now()),
        //     {
        //         coordinates: {
        //             latitude: 45,
        //             longitude: 20.5,
        //             values: [20.5, 45]
        //         }
        //     },
        //     new UserSummary(
        //         'nesto',
        //         'Aleksandar Stojkovic',
        //         'a@elfak.rs',
        //         4
        //     ),
        //     new SpeciesSummary(
        //         "aaa",
        //         Klasa.Birds,
        //         "duck",
        //         "duckus",
        //         `https://m.media-amazon.com/images/I/51VXgNZFIoL.jpg`,
        //         'sve ume',
        //         'protected'
        //     ),
        //     'https://nationalzoo.si.edu/sites/default/files/styles/wide/public/animals/ruddy-duck-male-20210402-817a2187-003rp.jpg?itok=izfpFvYK'
        // )

        // const sig2: Sighting = new Sighting(
        //     "testid",
        //     new Date(Date.now()),
        //     {
        //         coordinates: {
        //             latitude: 45,
        //             longitude: 20.5001,
        //             values: [20.5001, 45]
        //         }
        //     },
        //     new UserSummary(
        //         'nesto',
        //         'Aleksandar Stojkovic',
        //         'a@elfak.rs',
        //         4
        //     ),
        //     new SpeciesSummary(
        //         "aaa",
        //         Klasa.Birds,
        //         "duck",
        //         "duckus",
        //         `https://m.media-amazon.com/images/I/51VXgNZFIoL.jpg`,
        //         'sve ume',
        //         'protected'
        //     ),
        //     'https://nationalzoo.si.edu/sites/default/files/styles/wide/public/animals/ruddy-duck-male-20210402-817a2187-003rp.jpg?itok=izfpFvYK'
        // )
        // setSightingsState([sig1, sig2])
    }, [])

    useEffect(() => {
        if (sightingsState.length !== 0) {
            const pom = [];
            for (let i = 0; i < sightingsState.length; i++) {
                const iconMarkup = renderToStaticMarkup(<div><SightingMarker sighting={sightingsState[i]} /></div>);
                const customMarkerIcon = divIcon({
                    html: iconMarkup
                });
                pom.push(customMarkerIcon);
            }
            setSightingMarkersState(pom);
        }
    }, [sightingsState])

    useEffect(() => {
        console.log("Sta")
        if (habitatsState.length !== 0) {
            let centers: LocationI[] = [];
            let radiuses: number[] = [];
            for (let i = 0; i < habitatsState.length; i++) {
                let latSum = 0;
                let lngSum = 0;
                for (let j = 0; j < habitatsState[i].sightings.length; j++) {
                    latSum += habitatsState[i].sightings[j].location.coordinates.latitude
                    lngSum += habitatsState[i].sightings[j].location.coordinates.longitude
                }
                const latAvg = latSum / habitatsState[i].sightings.length
                const lngAvg = lngSum / habitatsState[i].sightings.length
                let radius = 0
                for (let j = 0; j < habitatsState[i].sightings.length; j++) {
                    if (Math.abs(habitatsState[i].sightings[j].location.coordinates.latitude - latAvg) > radius) {
                        radius = Math.abs(habitatsState[i].sightings[j].location.coordinates.latitude - latAvg)
                    }
                    if (Math.abs(habitatsState[i].sightings[j].location.coordinates.longitude - lngAvg) > radius) {
                        radius = Math.abs(habitatsState[i].sightings[j].location.coordinates.longitude - lngAvg)
                    }
                }
                radius += 0.0001
                console.log(radius * 111000);
                radiuses.push(radius)
                centers.push({
                    coordinates: {
                        latitude: latAvg,
                        longitude: lngAvg,
                        values: [lngAvg, latAvg]
                    }
                })
            }
            setHabitatsCenterState(centers);
            setHabitatsRadiusState(radiuses);
        }

    }, [habitatsState])

    return (
        <div className='w-screen h-screen'>
            <Bar />
            <MapContainer style={{ height: 'calc(100vh - 63px)', width: '100%', background: '#8bc34a', }} className="z-10" center={defaultCenter} zoom={defaultZoom} scrollWheelZoom={true}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <MarkerClusterGroup
                    chunkedLoading>
                    {sightingMarkersState.length !== 0 && sightingsState.map((s, index) => {
                        return <Marker riseOnHover
                            position={[s.location.coordinates.latitude, s.location.coordinates.longitude]} icon={sightingMarkersState[index]} key={s.id}>
                            <Popup><div className="flex flex-col w-44">
                                <img src={s.imageUrl} alt={s.species.commonName} style={{ width: `100px` }} />
                                <span>Common name: {s.species.commonName}</span>
                                <span>Scientific name: {s.species.scientificName}</span>
                                <span>Description: {s.species.description}</span>
                                <span>Conservation: {s.species.conservationStatus}</span>
                                <span>Photographed by: {s.sighter.name}</span>

                            </div></Popup>
                        </Marker>
                    })}
                </MarkerClusterGroup>

                {habitatsState.length !== 0 && habitatsCenterState.length !== 0 && habitatsRadiusState.length !== 0 && habitatsState.map((h, index) => {
                    // const map=useMap()
                    // L.circle([habitatsCenterState[index].coordinates.latitude, habitatsCenterState[index].coordinates.longitude],habitatsRadiusState[index]).addTo(map);
                    // <HabitatCircle lat={habitatsCenterState[index].coordinates.latitude} lng={habitatsCenterState[index].coordinates.longitude} radius={habitatsRadiusState[index]*111000}/>
                    // return true
                    console.log("here");
                    return <Circle key={index} center={[habitatsCenterState[index].coordinates.latitude, habitatsCenterState[index].coordinates.longitude]} pathOptions={{ fillColor: 'green' }} radius={habitatsRadiusState[index] * 111000} />
                })}
            </MapContainer>
        </div>
    )
}

export default Map



