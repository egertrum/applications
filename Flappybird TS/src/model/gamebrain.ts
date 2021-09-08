import GameScore from "./statisticsbrain";
import Bird from "./bird";

const CELL_PATH = 0;
const CELL_OBSTACLE = -1;

const MIN_OBSTACLE_LEN = 6; //minimum amount of blocks form top and bottom  
const OBS_WIDTH = 8;
const OBSTACLE_COUNT = 3; //obstacles on one page

const COL_COUNT = 100;
const ROW_COUNT = 100;

const PATH_WIDTH = Math.round(ROW_COUNT * 0.4);

const WHITE = 1;
const BLACK = 2;
const GRAY = 3;


export default class GameBrain {
    private countOfSpacesBetweenObs = 0;
    private sky = true;
    private obstacleThickness: number = OBS_WIDTH;
    private pathChange = 0;
    private score = -2;
    private board: number[][] = [];
    private colCount: number = COL_COUNT;
    private rowCount: number = ROW_COUNT;

    private gamescore: GameScore;
    private bird: Bird;


    constructor(gameScore: GameScore) {
        this.colCount = COL_COUNT;
        this.rowCount = ROW_COUNT;

        this.bird = new Bird(ROW_COUNT, COL_COUNT);
        this.intializeBoard();

        this.gamescore = gameScore;
    }

    getScore(): number { 
        if (this.score < 0) {
            return 0;
        }
        return this.score;
    }

    saveScore(name: string): void {
        this.gamescore.addToLeaderBoard(name, this.getScore());
    }

    birdFly(): void {
        this.bird.fly();
    }

    birdMove(): boolean {
        return this.bird.move(this.board, CELL_OBSTACLE, ROW_COUNT);
    }

    birdMatrix(): void {
        this.bird.matrix();
    }

    getBirdBox(): number[][] { return this.bird.getBox(); }

    getObstacleCount(): number { return this.colCount / OBSTACLE_COUNT; }

    getRowCount(): number { return this.rowCount; }
    getColCount(): number { return this.colCount; }

    createGameRow(pathPosition: number, sky: boolean): number[] {
        let res = [];
        for (let index = 0; index < this.rowCount; index++) {
            if (sky) {
                res.push(CELL_PATH); 
            } else {
                switch (true) {
                    case index < pathPosition:
                        res.push(CELL_OBSTACLE);
                        break;
                    case index >= pathPosition + PATH_WIDTH:
                        res.push(CELL_OBSTACLE);
                        break;
                    default:
                        res.push(CELL_PATH);
                        break;
                }   
            }
        }
        return res;
    }

    run(): void {
        this.board.splice(0, 1);

        if ((this.countOfSpacesBetweenObs + this.obstacleThickness) < this.getObstacleCount() && this.sky) {
            this.board.push(this.createGameRow(0, true));
            this.countOfSpacesBetweenObs++;
        } else {
            this.obstacleThickness--;

            if(this.obstacleThickness == (OBS_WIDTH - 1)) {
                this.sky = false;
                this.pathChange = Math.random() * ((this.rowCount - (PATH_WIDTH + MIN_OBSTACLE_LEN)) - (MIN_OBSTACLE_LEN - 1)) + (MIN_OBSTACLE_LEN - 1);
            } else if (this.obstacleThickness == 0) {
                this.obstacleThickness = OBS_WIDTH;
                this.countOfSpacesBetweenObs = 0;
                this.score++;
                this.sky = true;
            }
            this.board.push(this.createGameRow(this.pathChange, false));
        }
    }

    intializeBoard() {

        this.countOfSpacesBetweenObs = 0;
        this.sky = true;
        this.obstacleThickness = OBS_WIDTH;
        this.pathChange = 0;
        this.score = -2;
        this.board = [];

        this.bird = new Bird(ROW_COUNT, COL_COUNT);

        this.birdMatrix();

        for (let index = 0; index <= this.colCount; index++) {
                this.board.push(this.createGameRow(0, true));
        }
    }

    getGameBoard(): number[][] { return this.board; }
    gameCellPath(): number { return CELL_PATH; }
    gameCellObstacle(): number { return CELL_OBSTACLE; }
    getBirdXStart(): number { return this.bird.getBirdXStart(); }
    birdPos(): number { return this.bird.getBirdYStart(); }

}
