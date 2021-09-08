import { createStore } from 'vuex'

export interface IState {
    token: string | null;
    firstname: string;
    lastname: string;
    role: string;
}

export const initialState: IState = {
    token: null,
    firstname: '',
    lastname: '',
    role: ''
}

export interface IJwtResponse {
    token: string;
    firstname: string;
    lastname: string;
    role: string;
}

export interface ILoginInfo {
    email: string;
    password: string;
}

export default createStore({
    state: initialState,
    mutations: {
        logOut: (state: IState) => {
            state.token = null;
            state.firstname = '';
            state.lastname = '';
            state.role = '';
        },
        logIn: (state: IState, jwtResponse: IJwtResponse) => {
            state.token = jwtResponse.token;
            state.firstname = jwtResponse.firstname;
            state.lastname = jwtResponse.lastname;
            state.role = jwtResponse.role;
        },
    },
    actions: {
    },
    getters: {
    },
    modules: {
    }
})
