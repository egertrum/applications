import {HttpClient, inject } from "aurelia"
import { IJoke } from "../domain/IJoke";


@inject()
export class JokeService {

    private message: string;
    public loading: boolean = true;
    constructor(private httpClient: HttpClient) {
    }

    async getJoke(category: string): Promise<IJoke> {
        //var joke = [] // ilma selleta error, kuna korraks returnib undefined ja siis alles tuleb info, timeout maybe or load?
        return this.httpClient
            .get("https://api.chucknorris.io/jokes/random?category=" + category, { cache: "no-store" })
            .then(response => {
                return response.json();
            })
            .then(data => {
                return data;
            })
            .catch(error => []);
    }
}