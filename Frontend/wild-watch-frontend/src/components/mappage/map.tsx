import React, { useRef } from 'react'
import Bar from '../crucials/Bar'
import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet'
import { useTheme } from '@emotion/react'
import { useMapContext } from '../../contexts/map.context'
import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';
import L from 'leaflet'

let DefaultIcon = L.icon({
    iconUrl: icon,
    shadowUrl: iconShadow
});

L.Marker.prototype.options.icon = DefaultIcon
const Map: React.FC = () => {
    let defaultCenter = { lat: 45, lng: 20.5 };
    let defaultZoom = 13

    const { setSightings, setHabitats, sightings, habitats } = useMapContext();
    if(habitats.length===0 && sightings.length===0){
        
    }

    return (
        <div className='w-screen h-screen'>
            <Bar />
            <MapContainer style={{ height: 'calc(100vh - 63px)', width: '100%', background: '#8bc34a', }} className="z-10" center={defaultCenter} zoom={defaultZoom} scrollWheelZoom={true}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                {/* <Marker position={position}>
                    <Popup>
                        A pretty CSS3 popup. <br /> Easily customizable.
                    </Popup>
                </Marker> */}
            </MapContainer>
        </div>
    )
}

export default Map