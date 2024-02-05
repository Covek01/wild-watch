import React, { useRef } from 'react'
import Bar from '../crucials/Bar'
import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet'

const Map: React.FC = () => {
    const mapRef = useRef(null);
    let defaultCenter = { lat: 45, lng: 20.5 };
    let defaultZoom = 13
    return (
        <div className='w-screen h-screen'>
            <Bar />
            <MapContainer style={{ height: 'calc(100vh - 63px)', width: '100%', background: 'green', }} ref={mapRef} className="z-10" center={defaultCenter} zoom={defaultZoom} scrollWheelZoom={true}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />

            </MapContainer>
        </div>        
    )
}

export default Map