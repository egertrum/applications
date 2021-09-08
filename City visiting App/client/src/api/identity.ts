import axios from "axios";
import { apiUrl } from "../configuration";
import IUser from "../types/IUser";
import IJwt from "../types/IJwt";

const API = axios.create({ baseURL: apiUrl + "user/"});

export const getUser = (id: string) => API.get(id);
export const signIn = (data: IUser) => API.post("signin", data);
export const signUp = (data: IUser) => API.post("signup", data);