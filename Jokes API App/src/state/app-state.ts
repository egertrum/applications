import { IJoke } from "../domain/IJoke";
import { ITodo } from "../domain/ITodo";
import { JokeService } from '../services/joke-service';

export class AppState {
    public todos: readonly ITodo[] = [];
    private moneyJokes: IJoke[] = [];
    private sportJokes: IJoke[] = [];
    private musicJokes: IJoke[] = [];

    constructor() {
    }

    async addJokes(category: string, service: JokeService): Promise<void> {
        for (let i = 0; i < 5; i++) {
            service.getJoke(category).then(data => {
                switch (category) {
                    case "money":
                        if (!this.moneyJokes.some(joke => joke.id === data.id)) {
                            data.duplicates = 0;
                            this.moneyJokes.unshift(data);
                        } else {
                            let joke = this.moneyJokes.find(joke => joke.id === data.id)
                            joke.duplicates++;
                        }
                        break;
                    case "music":
                        if (!this.musicJokes.some(joke => joke.id === data.id)) {
                            data.duplicates = 0;
                            this.musicJokes.unshift(data);
                        } else {
                            let joke = this.musicJokes.find(joke => joke.id === data.id)
                            joke.duplicates++;
                        }
                        break;
                    case "sport":
                        if (!this.sportJokes.some(joke => joke.id === data.id)) {
                            data.duplicates = 0;
                            this.sportJokes.unshift(data);
                        } else {
                            let joke = this.sportJokes.find(joke => joke.id === data.id)
                            joke.duplicates++;
                        }
                        break;
                }
            });
        }
    }

    /*
    getDuplicates(): number {
        return this.duplicates;
    }

    initalizeDuplicates(): void {
        this.duplicates = 0;
    }
    */

    getMoneyTodos(): IJoke[] {
        return this.moneyJokes;
    }

    getSportTodos(): IJoke[] {
        return this.sportJokes;
    }

    getMusicTodos(): IJoke[] {
        return this.musicJokes;
    }

    addTodo(todo: ITodo): void {
        this.todos = [...this.todos, todo];
    }

    removeTodo(elemNo: number): void {
        this.todos = this.todos.filter((elem, index) => index !== elemNo);
    }

    countToDos(): number {
        return this.todos.length;
    }
}
