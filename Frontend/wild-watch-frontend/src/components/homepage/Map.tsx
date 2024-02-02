import {
    MapContainer,
    TileLayer,
    useMap,
  } from 'react-leaflet'
import React from 'react'
//import 'leaflet/dist/leaflet.css';


const Map:React.FC = () => {
    const position = [51.505, -0.09]

    return(
        <>
            <MapContainer center={{lat: position[0], lng: position[1]}} zoom={13} scrollWheelZoom={true}>
                <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
            </MapContainer>
        </>
    )
}

export default Map