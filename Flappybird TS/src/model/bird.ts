const BIRD_HEIGHT_PERCETNAGE = 0.08; // rowcount * this
const BIRD_WIDTH = 8;

const FLY_TIME = 10;


export default class Bird{
    private birdUp = false;
    private upTime: number = FLY_TIME;

    private birdTopY: number;
    private birdBottomY: number;
    private birdStartColumn: number;
    private birdEndColumn: number;
    private birdBox: number[][] = [];

    constructor(rowCount: number, colCount: number) {
        this.birdTopY = Math.round((rowCount / 2) - ((rowCount * BIRD_HEIGHT_PERCETNAGE) / 2));
        this.birdBottomY = this.birdTopY + (rowCount * BIRD_HEIGHT_PERCETNAGE);
        this.birdStartColumn = Math.round(colCount * 0.25);
        this.birdEndColumn = this.birdStartColumn + BIRD_WIDTH;

    }

    getBirdXStart(): number { return this.birdStartColumn; }
    getBirdYStart(): number { return this.birdTopY; }
    getBirdXEnd(): number { return this.birdEndColumn; }
    getBirdYEnd(): number { return this.birdBottomY; }

    getBox(): number[][] { return this.birdBox; }


    fly(): void {
        this.birdUp = true;
    }

    move(board: number[][], obs: number, rowCount: number): boolean {
        if (!this.birdUp) {
            this.birdTopY++;
            this.birdBottomY++;
        } else {
            if (this.upTime != 0) {
                this.birdTopY-=2;
                this.birdBottomY-=2;
                this.upTime--;
            } else {
                this.birdUp = false;
                this.upTime = FLY_TIME;
            }
        }

        return this.checkHit(board, obs, rowCount);
    }


    matrix(): void {
        this.birdBox = [
            [0,0,3,3,3,3,3,0],
            [0,3,3,3,2,3,2,3],
            [3,0,0,3,3,3,3,3],
            [0,0,0,3,3,2,2,3],
            [0,0,0,0,3,2,2,3],
            [0,0,0,0,3,2,3,3],
            [0,0,0,3,3,3,3,0],
            [3,3,3,3,3,0,0,0]
        ];
        /*
        for (let y = 0; y < BIRD_HEIGHT; y++) {
            let row = [];
            for (let x = 0; x < BIRD_WIDTH; x++) {
                row.push(8);
            }
            this.birdBox.push(row);
        }
        */
    }

    checkHit(board: number[][], obs: number, rowCount: number): boolean {
        for (let v = this.birdTopY; v < this.birdBottomY; v++) {
            if (board[this.birdEndColumn][v] === obs) {
                return true;
            }
        }
        for (let i = this.birdStartColumn; i < this.birdEndColumn; i++) {
            if (board[i][this.birdTopY] === obs || board[i][this.birdBottomY] === obs) {
                return true;
            }
        }
        return (this.birdBottomY > rowCount || this.birdTopY < 0);
    }
}