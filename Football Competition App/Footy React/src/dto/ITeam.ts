import { ICountry } from "./ICountry";

export interface ITeam {
    id?: string | null,
    countryId: string,
    country?: ICountry,
    appUserId: string,
    name: string,
    playersAmount: string
}