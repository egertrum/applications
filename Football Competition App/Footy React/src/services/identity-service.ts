import { IJwtResponse } from './../types/IJwtResponse';
import Axios, { AxiosError } from 'axios';
import { ApiBaseUrl } from '../configuration';
import { IFetchResponse } from '../types/IFetchResponse';
import { IMessages } from '../types/IMessages';
import { IRegister } from '../dto/IRegister';

export abstract class IdentityService {
    protected static axios = Axios.create({
        baseURL: ApiBaseUrl,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    static async Login(apiEndpoint: string, loginData: {email: string, password:string}): Promise<IFetchResponse<IJwtResponse>> {
        let loginDataJson = JSON.stringify(loginData);
        try {
            let response = await this.axios.post<IJwtResponse>(apiEndpoint, loginDataJson);
            return {
                ok: response.status <= 299,
                statusCode: response.status,
                data: response.data
            };    
        }
        catch (err) {
            let error = err as AxiosError;
            return {
                ok: false,
                statusCode: error.response?.status ?? 500,
                messages: (error.response?.data as IMessages).messages,
            }
        }
    }

    static async Register(apiEndpoint: string, regiserData: IRegister): Promise<IFetchResponse<IJwtResponse>> {
        let regiserDataJson = JSON.stringify(regiserData);
        try {
            let response = await this.axios.post<IJwtResponse>(apiEndpoint, regiserDataJson);
            return {
                ok: response.status <= 299,
                statusCode: response.status,
                data: response.data
            };    
        }
        catch (err) {
            let error = err as AxiosError;
            return {
                ok: false,
                statusCode: error.response?.status ?? 500,
                messages: (error.response?.data as IMessages).messages,
            }
        }
    }

}
