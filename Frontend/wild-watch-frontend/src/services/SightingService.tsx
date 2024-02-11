import { api } from "./Service"
import axios from "axios";
import { Sighting, SightingDto } from "../models/Sighting";

class SightingService{
    public async GetSightingsBySighter(id: string){
        try{
            const {data, status} = await api.get<Sighting[]>(`sighting/getBySighter/${id}`)

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

    public async GetMySightings(){
        try{
            const {data, status} = await api.get<Sighting[]>(`sighting/getMySightings`)

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

    public async CreateSighting(body: SightingDto){
        try{
            const {data, status} = await api.post<string>(`sighting/create`, body)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return data
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while getting species info: ', error.message);
                
                return '';
            } else {
                console.log('unexpected error while getting species info: ', error);

                //const errObject: Location = new Location(0, '')
                return '';
            }
        }
    }
}

export default new SightingService