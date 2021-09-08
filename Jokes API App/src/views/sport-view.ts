import { JokeService } from './../services/joke-service';
import { IJoke } from "../domain/IJoke";
import { AppState } from "../state/app-state";

export class SportView {
    private data: IJoke[] = [];
    private message: string;

    constructor(private jokeService: JokeService,
        private appState: AppState) {
    }

    //private number: number = this.appState.

    async attached() {
        this.appState.addJokes("sport", this.jokeService);
        /*
        let numOfAddedJokes: string = (5 - duplicates).toString();
        this.message = "Here are " + numOfAddedJokes + " new jokes for you!"
        if (duplicates > 0) { this.message += " Instead of 5 jokes you got " + numOfAddedJokes + " because there were " + duplicates + " duplicate(s)." }
        */
        this.data = this.appState.getSportTodos();
    }
}
