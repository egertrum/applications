import { IPerson } from "./IPerson";
import { IRole } from "./IRole";
import { ITeam } from "./ITeam";

export interface ITeamPerson {
    id?: string | null,
    teamId: string,
    team?: ITeam,
    personId: string,
    person?: IPerson,
    roleId: string,
    role?: IRole,
    since?: Date | null,
    until?: Date | null
}