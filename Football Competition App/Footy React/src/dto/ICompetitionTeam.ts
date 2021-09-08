import { ICompetition } from "./ICompetition";
import { ICountry } from "./ICountry";
import { ITeam } from "./ITeam";

export interface ICompetitionTeam {
    id?: string | null,
    teamId: string,
    team?: ITeam,
    competitionId: string,
    competition?: ICompetition,
    since?: Date
}