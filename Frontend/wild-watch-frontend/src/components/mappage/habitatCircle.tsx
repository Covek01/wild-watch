import L from "leaflet";
import { useMap } from "react-leaflet";

interface HabitatCircleProps {
    lat: number,
    lng: number,
    radius: number
}
export default function HabitatCircle({ lat, lng, radius }: HabitatCircleProps) {
    const map = useMap()
    L.circle([lat, lng], {
        radius:radius}).addTo(map);
    return null;
}