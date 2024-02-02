import { api } from "./Service"
import { Species } from "../models/Species"
import axios from "axios";

class SpeciesService{
    public async GetSpeciesInfo(id: string){
        try{
            const {data, status} = await api.get<Species>(`species/read/${id}`)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return data
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while getting species info: ', error.message);
                
                return null;
            } else {
                console.log('unexpected error while getting species info: ', error);

                //const errObject: Location = new Location(0, '')
                return null;
            }
        }
    }
}

export default new SpeciesService