import axios from "axios"
import { Habitat } from "../models/Habitat"
import { api } from "./Service"


 class HabitatService {
    public async GetHabitatsWithNumberOfSightings() {
        const minSighints = 3
        try {
            const { data, status } = await api.get<Habitat[]>(`/habitat/readMinSize/${minSighints}`)
            
            return { data: data, status: status }
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.error(`getHabitatsWithNumberOfSightings error message:`, error.message);

                return { data: null, status: null }
            } else {
                console.error(`unexpected getHabitatsWithNumberOfSightings error`, error)

                return { data: null, status: null }
            }
        }
    }

   
}

export default new HabitatService
