export default function mainView(eventHandler: any) {
    let control = document.createElement('div');
    control.id = 'control';

    let statisticsButton = document.createElement('button');
    statisticsButton.id = 'statistics';
    statisticsButton.innerText='Leaderboard';
    statisticsButton.style.marginRight = "5px";


    let gameButton = document.createElement('button');
    gameButton.id = 'game';
    gameButton.innerText='New Game';
    gameButton.style.marginLeft = "5px";

    control.append(statisticsButton);
    control.append(gameButton);

    let score = document.createElement('div');
    score.style.backgroundColor = "#ADD8E6";
    score.style.display = "inline-block";
    score.style.marginLeft = "20px";
    score.id = "score";

    control.append(score);

    control.style.textAlign = "center";

    statisticsButton.addEventListener('click', () => {eventHandler((statisticsButton))});
    gameButton.addEventListener('click', () => {eventHandler((gameButton))});
    
    return control;
}
