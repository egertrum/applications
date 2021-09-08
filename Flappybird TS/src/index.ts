import mainView from './views/mainview';
import controlView from './views/controlview';
import gameView from './views/gameview';

import GameController from './controllers/game-controller';
import StatisticsController from './controllers/statistics-controller';

import GameBrain from './model/gamebrain';
import GameScore from './model/statisticsbrain';

let game_view = gameView();
let gameController = new GameController(game_view);
let statisticsController = new StatisticsController(game_view);

let gameScore = new GameScore();

let view = mainView();
document.body.append(view);
let ctrl_view = controlView(gameControlClick);
view.append(ctrl_view);
view.append(game_view);

function gameControlClick(e: HTMLButtonElement): void {
    e.blur();
    switch (e.id) {
        case 'game':
            gameController.stop();
            statisticsController.stop();
            gameController.run(new GameBrain(gameScore));
            break;
        case 'statistics':
            gameController.stop();
            statisticsController.run(gameScore);
            break;

        default:
            break;
    }
}

statisticsController.run(gameScore);

window.addEventListener('resize', () => {
    gameController.resizeUi();
    statisticsController.resizeUi();
});

window.addEventListener('keyup', () => {
    gameController.birdUp();
});