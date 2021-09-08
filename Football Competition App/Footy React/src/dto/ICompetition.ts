import { ICountry } from "./ICountry";

export interface ICompetition {
    id?: string | null,
    countryId: string,
    country?: ICountry | null,
    name: string,
    organiser: string,
    startDate?: Date | null,
    endDate?: Date | null,
    comment?: string | null
}