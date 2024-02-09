import L from "leaflet";
import { useEffect, useState } from "react";
import { Marker, Popup, useMap } from "react-leaflet";

interface LocatorProps {
    setDefaultCenterState: React.Dispatch<React.SetStateAction<{ lat: number, lng: number }>>
}
export default function Locator({ setDefaultCenterState }: LocatorProps) {
    const [position, setPosition] = useState<L.LatLng>();

    const map = useMap();
    let locationIcon = L.icon({
        iconSize: [40, 40],
        iconAnchor: [20, 40],
        popupAnchor: [0, -40],
        iconUrl: './pin.png',
        shadowUrl: "",
    });
    useEffect(() => {
        map.locate().on("locationfound", function (e) {
            setDefaultCenterState({ lat: e.latlng.lat, lng: e.latlng.lng });
            setPosition(e.latlng)
            map.panTo(e.latlng);
        });
    }, []);

    return (
        <Marker position={position ?? { lat: 45, lng: 20.5 }} icon={locationIcon}>
            <Popup>You are here</Popup>
        </Marker>
    );
}
