import { combineReducers } from "redux";
import posts from "./posts";
import identity from "./identity";

export default combineReducers({
    posts,
    identity
});