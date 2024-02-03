import axios from "axios"
//import { lsGetSession } from "../utils/helpers"
import { error } from "console"
import { lsGetToken } from "../utils/helpers";

export const constants = {
    apiName: `https://localhost:7139`,
};

const axiosInstance = axios.create({
    baseURL: constants.apiName,
});

axiosInstance.interceptors.request.use(
    (request) => {
        const token = lsGetToken();

        if(token){
            request.headers!["Authorization"]=`Bearer ${token}`;
        }

        return request;
    },
    (error)=>{
        Promise.reject(error);
    }
);

export const api = axiosInstance