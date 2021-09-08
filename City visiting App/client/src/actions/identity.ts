import { IDENTITY, ERROR } from '../types/actionTypes';
import * as api from '../api/identity';
import IUser from '../types/IUser';
import { History } from 'history';

export const signin = (formData: IUser, router: History<unknown> | string[]) => async (dispatch: any) => {
  try {
    const { data } = await api.signIn(formData);

    dispatch({ type: IDENTITY, data });

    router.push('/');
  } catch (error) {
    dispatch({ type: ERROR, error });
  }
};

export const signup = (formData: IUser, router: History<unknown> | string[]) => async (dispatch: any) => {
  try {
    const { data } = await api.signUp(formData);

    dispatch({ type: IDENTITY, data });

    router.push('/');
  } catch (error) {
    dispatch({ type: ERROR, error });
  }
};