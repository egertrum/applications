import * as api from "../api";
import { FETCH_ALL, CREATE, UPDATE, DELETE, ERROR } from "../types/actionTypes";
import IPost from "../types/IPost";

// Action Creators
export const getPosts = () => async (dispatch: any) => {

    try {
        const { data } = await api.fetchPosts();
        // payload basically data where to store posts rn
        dispatch({ type: FETCH_ALL, payload: data })
    } catch (error) {
        dispatch({ type: ERROR, error });
    }
}

export const createPost = (post: IPost) => async (dispatch: any) => {

    try {
        const { data } = await api.createPost(post);
        // payload basically data where to store posts rn
        dispatch({ type: CREATE, payload: data });
    } catch (error) {
        dispatch({ type: ERROR, error });
    }
}

export const updatePost = (id: string, post: IPost) => async (dispatch: any) => {
    try {
        const { data } = await api.updatePost(id, post);

        dispatch({ type: UPDATE, payload: data });
    } catch (error) {
        dispatch({ type: ERROR, error });
    }
};

export const deletePost = (id: string) => async (dispatch: any) => {
    try {
        await api.deletePost(id);
        dispatch({ type: DELETE, payload: id });
    } catch (error) {
        dispatch({ type: ERROR, error });
    }
};

export const likePost = (id: string) => async (dispatch: any) => {
    try {
        const { data } = await api.likePost(id);

        dispatch({ type: UPDATE, payload: data });
    } catch (error) {
        dispatch({ type: ERROR, error });
    }
};