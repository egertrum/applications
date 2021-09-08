import { ICompetition } from "./ICompetition";
import { ICountry } from "./ICountry";
import { IGameType } from "./IGameType";
import { ITeam } from "./ITeam";

export interface IGame {
    id?: string | null,
    homeId: string,
    home?: ITeam,
    awayId: string,
    away?: ITeam,
    competitionId: string,
    competition?: ICompetition,
    gameTypeId: string,
    gameType?: IGameType,
    kickOffTime?: Date | string | null,
    homeScore?: number | null,
    awayScore?: number | null,
    comment?: string | null,
    idOfGameWinner?: string | null
}