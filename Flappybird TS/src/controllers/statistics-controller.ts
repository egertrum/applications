import GameScore from "../model/statisticsbrain";

export default class StatisticsController {
    viewContainer: HTMLDivElement;
    isRunning: boolean;
    score: GameScore | undefined;
    scoreBox: any;

    constructor(viewContainer: HTMLDivElement) {
        this.viewContainer = viewContainer;
        this.isRunning = false;
    }

    run(gameScore: GameScore){
        console.log(this.viewContainer);  
        this.isRunning = true;
        this.score = gameScore;
        this.viewContainer.innerHTML = '';
        this.viewContainer.append(this.getLeaderBoardHtml())
    }

    stop(){
        this.isRunning = false;
    }
    resizeUi(){
        if (this.isRunning){
            // redraw
        }
    }
    getLeaderBoardHtml() {
        let content = document.createElement('table');
        content.id = "leaderboard";
        content.style.backgroundColor = "#FFE4B5";
        //content.classList = "center";
        content.style.width = "100%";
        content.style.height = "100%";

        let headerRow = document.createElement('tr');
        let headerNameTitle = document.createElement('th');
        let headerScoreTitle = document.createElement('th');
        headerNameTitle.innerText = "Name";
        headerScoreTitle.innerText = "Score";

        headerRow.append(headerNameTitle);
        headerRow.append(headerScoreTitle);

        content.append(headerRow);

        this.scoreBox = this.score!.getLeaderBoard();

        for(let name in this.scoreBox) {
            let scoreRow = document.createElement('tr');
            //scoreRow.style.border = "3px solid black";
            var score = this.scoreBox[name];
            let nameToAdd = document.createElement('td');
            let scoreToAdd = document.createElement('td');

            nameToAdd.innerText = name;
            scoreToAdd.innerText = score;

            scoreRow.append(nameToAdd);
            scoreRow.append(scoreToAdd);

            content.append(scoreRow);
        }
    

        return content;
    }
}
