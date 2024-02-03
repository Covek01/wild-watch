import axios from "axios"
import { User } from "../models/User"
import { api } from "./Service"
import { domainToASCII } from "url"
import { stat } from "fs"
import { UserRegister } from "../dtos/userRegister"

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

}

export default new UserService