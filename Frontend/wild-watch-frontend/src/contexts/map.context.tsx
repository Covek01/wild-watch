import { ReactNode, useContext, createContext, useState } from "react";
import { Habitat } from "../models/Habitat";
import { Sighting } from "../models/Sighting";

interface MapStateContextI {
    sightings: Sighting[];
    habitats: Habitat[];
    setSightings: (sightings: Sighting[]) => void;
    setHabitats: (habitats: Habitat[]) => void;
}

interface MapStateProviderProps {
    children: ReactNode;
}

const MapStateContext = createContext<MapStateContextI>(
    {} as MapStateContextI
)

export function useMapContext() {
    const context = useContext(MapStateContext)
    if (!context)
        throw new Error("useMapState must be used within MapProvider");

    return context;
}

export function MapStateProvider({ children }: MapStateProviderProps) {
    const [sightingsState, setSightingsState] = useState<Sighting[]>([]);
    const [habitatsState, setHabitatsState] = useState<Habitat[]>([]);

    const setSightings = (sightings: Sighting[]) => {
        setSightingsState(sightings);
    }
    const setHabitats = (habitats: Habitat[]) => {
        setHabitatsState(habitats);
    }

    return (
        <MapStateContext.Provider
            value={{
                setSightings,
                setHabitats,
                sightings: sightingsState,
                habitats: habitatsState
            }}
        >
            {children}
        </MapStateContext.Provider>
    )
}