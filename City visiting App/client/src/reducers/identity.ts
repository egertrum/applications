import * as actionType from '../types/actionTypes';

const identityReducer = (state = { authData: null }, action: any) => {
  switch (action.type) {
    case actionType.IDENTITY:
      localStorage.setItem('profile', JSON.stringify({ ...action?.data }));

      return { ...state, authData: action.data, loading: false, errors: null };
    case actionType.LOGOUT:
      localStorage.clear();

      return { ...state, authData: null, loading: false, errors: null };
    case actionType.ERROR:
      return { ...state, authData: null, loading: false, errors: action.data };
    default:
      return state;
  }
};

export default identityReducer;