import axios from "axios"
import { User } from "../models/User"
import { api } from "./Service"
import { domainToASCII } from "url"
import { stat } from "fs"
import { UserRegister } from "../dtos/userRegister"
import { SightingSummaryUser } from "../models/Sighting"

class UserService {
    public async Signin(email: string, password: string) {
        try {
            const { data, status } = await api.post<User & { accessToken: string }>(`/user/login`, { email, password })
            // const user=new User(
            //     data.user.id,
            //     data.user.name,
            //     data.user.email,
            //     data.user.imageUrl,
            //     data.user.dateOfBirth,
            //     data.user.location,
            //     data.user.sightings,
            //     data.user.favouriteSpecies);
            // return {data:{user:user,accessToken:data.accessToken}, status:status }

            return { data: data, status: status };
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.error('signin error message:', error.message);

                return { data: null, status: null };
            } else {
                console.error('unexpected signin error message', error);
                return { data: null, status: null };
            }
        }
    }

    public async Signup(user: UserRegister) {
        try {
            const { data, status } = await api.post<User>(`/user/register`,{...user,imageUrl:""});
            return { data, status }
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.error(`signup error message`, error.message)
                return { data: null, status: null };
            } else {
                console.error(`unexpected signup error message`, error)
                return { data: null, status: null };
            }
        }
    }

    public async GetUserInfo(id: string){
        try{
            const {data, status} = await api.get<User>(`user/read/${id}`)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return data
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while getting user info: ', error.message);
                
                return null
            } else {
                console.log('unexpected error while getting user info: ', error);

                //const errObject: Location = new Location(0, '')
                return null
            }
        }
    }

    public async UpdateUserName(id: string, username: string){
        try{
            const {data, status} = await api.put(`user/setUserName/${id}/${username}`)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return true
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while updating user name: ', error.message);
                
                return false
            } else {
                console.log('unexpected error while updating user name: ', error);

                //const errObject: Location = new Location(0, '')
                return false
            }
        }
    }


    public async GetMySightings(){
        try{
            const {data, status} = await api.get<SightingSummaryUser[]>(`user/getMySightings/`)

            console.log(JSON.stringify(data, null, 4));
            console.log('response status is: ', status);

            return data
        }
        catch (error) {
            if (axios.isAxiosError(error)) {
                console.log('error message while updating user name: ', error.message);
                
                return null
            } else {
                console.log('unexpected error while updating user name: ', error);

                //const errObject: Location = new Location(0, '')
                return null
            }
        }
    }
}

export default new UserService