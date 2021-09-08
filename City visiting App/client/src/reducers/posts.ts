import { FETCH_ALL, CREATE, UPDATE, DELETE, LIKE } from "../types/actionTypes";
import IPost from "../types/IPost";

export default (posts: IPost[] = [], action: { type: any; payload: any; }) => {
    switch (action.type) {
        case FETCH_ALL:
            return action.payload;
        case CREATE:
            return [...posts, action.payload];
        case UPDATE:
        case LIKE:
            return posts.map((post: IPost) => (post._id === action.payload._id ? action.payload : post));
        case DELETE:
            return posts.filter((post: IPost) => (post._id !== action.payload));
        default:
            return posts;
    }
}