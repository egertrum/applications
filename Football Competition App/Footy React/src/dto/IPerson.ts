import { ICountry } from "./ICountry";

export interface IPerson {
    id?: string | null,
    appUserId: string,
    countryId: string,
    country?: ICountry,
    firstName: string,
    lastName: string,
    identificationCode: string,
    birthDate?: Date | string,
    gender?: string,
    personTeams?: []
}