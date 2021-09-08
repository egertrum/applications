import { IJwtResponse } from './../types/IJwtResponse';
import Axios, { AxiosError } from 'axios';
import { ApiBaseUrl } from '../configuration';
import { IFetchResponse } from '../types/IFetchResponse';
import { IRegister } from '../domain/IRegister';

export abstract class IdentityService {
    protected static axios = Axios.create({
        baseURL: ApiBaseUrl,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    static async Login(apiEndpoint: string, loginData: {email: string, password:string}): Promise<IFetchResponse<IJwtResponse>> {
        const loginDataJson = JSON.stringify(loginData);
        try {
            const response = await this.axios.post<IJwtResponse>(apiEndpoint, loginDataJson);
            return {
                ok: response.status <= 299,
                statusCode: response.status,
                data: response.data
            };
        } catch (err) {
            const error = err as AxiosError;
            return {
                ok: false,
                statusCode: error.response?.status ?? 500,
                //messages: typeof (error.response!.data) === 'string' ? error.response!.data : (error.response?.data as IMessages).messages.toString()
            }
        }
    }

    static async Register(apiEndpoint: string, registerData: IRegister): Promise<IFetchResponse<IJwtResponse>> {
        const loginDataJson = JSON.stringify(registerData);
        try {
            const response = await this.axios.post<IJwtResponse>(apiEndpoint, loginDataJson);
            return {
                ok: response.status <= 299,
                statusCode: response.status,
                data: response.data
            };
        } catch (err) {
            const error = err as AxiosError;
            return {
                ok: false,
                statusCode: error.response?.status ?? 500,
            }
        }
    }
}
