import { ICountry } from "./ICountry";
import { IPerson } from "./IPerson";
import { ITeam } from "./ITeam";

export interface IRole {
    id?: string | null,
    nameId: string,
    name: string,
    commentId: string,
    comment?: string
}