import axios from "axios"
//import { lsGetSession } from "../utils/helpers"
import { error } from "console"

export const constants = {
    apiName: `https://localhost:7139`,
};
  
const axiosInstance = axios.create({
    baseURL: constants.apiName,
});
  
//   axiosInstace.interceptors.request.use(
//     (config) => {
//       let session = lsGetSession();
//       if (session) {
//         // if (new Date(session.expires) > new Date()) {
//         //   lsRemoveSession();
//         //   return config;
//         // }
  
//         config["headers"] = config.headers ?? {};
//         // @ts-ignore
//         config.headers["Authorization"] = `SessionId ${session.id}`;
//         console.log("interceptor");
//       }
  
//       return config;
//     },
//     (error) => {
//       return Promise.reject(error);
//     }
//   );

export const api = axiosInstance