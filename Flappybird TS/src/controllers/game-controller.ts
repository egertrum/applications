import GameBrain from "../model/gamebrain";

export default class GameController {
    viewContainer: HTMLDivElement;
    isRunning: boolean;
    rowHeight: number;
    colWidth: number;
    model!: GameBrain;
    timer!: any;
    birdMatrix!: number[][];
    content!: HTMLDivElement;

    constructor(viewContainer: HTMLDivElement) {
        this.viewContainer = viewContainer;
        this.isRunning = false;
        this.rowHeight = 0;
        this.colWidth = 0;
    }
    
    run(GameBrain: GameBrain): void {
        this.isRunning = true;
        this.model = GameBrain;
        this.getDisplayComponents();
        this.animate();
    }

    animate(): void {
        this.timer = setTimeout(() => {
            this.model!.run();
            this.shuffleBoard();
            this.animate();
        }, 50);
    }

    getDisplayComponents(): void {
        this.viewContainer.innerHTML = '';
        this.viewContainer.append(this.getBoardHtml());
        this.birdMatrix = this.model!.getBirdBox();
    }

    shuffleBoard(): void {
        const gameboard = document.getElementById("gameboard");
        if (gameboard != null) { gameboard.removeChild(gameboard.firstChild!); }
        this.removeLastBird();

        if(this.model.birdMove() == true) {
            this.stop();
            let name = prompt("Enter Your name! Your score was: " + this.model.getScore());
            if (name != null) {
                this.model.saveScore(name);
            } 
            this.model.intializeBoard();
            this.getDisplayComponents();
        } else {
            this.getBird();
            this.changeScore();
            this.getNewCol();
        }
    }

    changeScore(): void {
        let scoreSpan = document.getElementById("score");
        if (scoreSpan != null) {scoreSpan.innerHTML = "SCORE: " + this.model.getScore();}
    }

    stop(): void {
        clearTimeout(this.timer);
        this.isRunning = false;

        let scoreSpan = document.getElementById("score");
        scoreSpan!.innerHTML = "";
    }

    resizeUi(): void {
        if (this.isRunning){ 
            this.viewContainer.innerHTML = '';
            this.viewContainer.append(this.getBoardHtml());
        }
    }

    getBoardHtml(): HTMLDivElement {
        this.getCellSize();
        this.content = document.createElement('div');
        this.content.id = "gameboard";

        this.model.getGameBoard().forEach((colData: number[]) => {
            let colElem = this.getColElement();
            colData.forEach((rowData: number) => {
                let rowElem = this.getRowElement(rowData);
                colElem.append(rowElem);
            });
            this.content.append(colElem);
        });
        return this.content;
    }

    getColElement(): HTMLDivElement {
        let colElem = document.createElement('div');
        colElem.style.minHeight = this.rowHeight + 'px';
        colElem.style.maxHeight = this.rowHeight + 'px';
        colElem.style.float = "left";
        return colElem;
    }

    getRowElement(rowData: number): HTMLDivElement {
        let rowElem = document.createElement('div');
        rowElem.style.minWidth = this.colWidth + 'px';
        rowElem.style.minHeight = this.rowHeight + 'px';
        rowElem.style.backgroundColor = this.getCellColor(rowData);
        return rowElem;
    }

    getCellColor(rowData: number): string {
        let color = "";
        if (rowData === this.model.gameCellObstacle()) {
            color = '#228B22';
        } else if (rowData === this.model.gameCellPath()) {
            color = '#00BFFF';
        } else if (rowData === 2) {
            color = "#000";
        } else if (rowData === 3) {
            color = "#C0C0C0";
        }
        return color;
    }

    getNewCol(): void {
        let board = this.model.getGameBoard();
        let colElem = this.getColElement();

        board[board.length - 1].forEach((rowData: number) => {
            let rowElem = this.getRowElement(rowData);
            colElem.append(rowElem);
        });

        this.content.append(colElem);
    }

    getCellSize(): void {
        this.rowHeight = (window.innerHeight - (document.getElementById('control')!.clientHeight)) / this.model.getRowCount();
        this.colWidth = (window.innerWidth - 25) / (this.model.getColCount());
    }

    removeLastBird(): void {
        let birdStart = this.model.getBirdXStart();
        let help = birdStart;
        let pos = this.model.birdPos();

        for (let x = 0; x < this.birdMatrix.length; x++) {
            for (let y = 0; y < this.birdMatrix[0].length; y++) {
                let birdCell = this.content.childNodes[birdStart - 1].childNodes[pos];
                (birdCell as HTMLDivElement).style.backgroundColor = this.getCellColor(0);
                birdStart++;
            }
            birdStart = help;
            pos++;
        }
    }

    getBird(): void {
        let birdStart = this.model.getBirdXStart();
        let help = birdStart;
        let pos = this.model.birdPos();
        
        for (let x = 0; x < this.birdMatrix.length; x++) {
            for (let y = 0; y < this.birdMatrix[0].length; y++) {
                let birdCell = this.content.childNodes[birdStart].childNodes[pos];
                (birdCell as HTMLDivElement).style.backgroundColor = this.getCellColor(this.birdMatrix[x][y]);
                birdStart++;
            }
            birdStart = help;
            pos++;
        }
    }

    birdUp(): void {
        this.model.birdFly();
    }
    
}
