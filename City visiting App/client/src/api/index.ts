import axios from "axios";
import { apiUrl } from "../configuration";
import IPost from "../types/IPost";

const API = axios.create({ baseURL: apiUrl});
const apiEndPoint = "posts";

API.interceptors.request.use((req) => {
    if(localStorage.getItem("profile")) {
        req.headers.authorization = "Bearer " + JSON.parse(localStorage.getItem("profile")!).token;
    }

    return req;
});

export const fetchPosts = () => API.get(apiEndPoint);
export const createPost = (data: IPost) => API.post(apiEndPoint, data);
export const updatePost = (id: string, updatedPost: IPost) => API.patch(`${apiEndPoint}/${id}`, updatedPost);
export const likePost = (id: string) => API.patch(`${apiEndPoint}/${id}/like`);
export const deletePost = (id: string) => API.delete(`${apiEndPoint}/${id}`);