export default class GameScore {
    scoreBoard: { [key: string]: number };
    constructor() {
        this.scoreBoard = {}; // of GameScore
    }

    addToLeaderBoard(name: string, score: number) {
        this.scoreBoard[name] = score;
    }

    getLeaderBoard() { return this.scoreBoard }
}
