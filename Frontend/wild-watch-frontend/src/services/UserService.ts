import { api } from "./Service"
import { User } from "../models/User"
import axios from "axios"

class UserService{
    public async GetSpeciesInfo(id: string){
        try{
            const {data, status} = await api.get<User>(`user/read/${id}`)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return data
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while getting user info: ', error.message);
                
                return null;
            } else {
                console.log('unexpected error while getting user info: ', error);

                //const errObject: Location = new Location(0, '')
                return null;
            }
        }
    }
}

export default new UserService