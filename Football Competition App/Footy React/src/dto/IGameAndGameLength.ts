import { IGame } from "./IGame";
import { IGameLength } from "./IGameLength";

export interface IGameAndGameLength {
    game: IGame,
    gameLength: IGameLength,
}