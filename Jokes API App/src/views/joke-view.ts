import { bindable } from "@aurelia/runtime-html";
import { IJoke } from "../domain/IJoke";

export class JokeView {
    @bindable public data: IJoke[];

    constructor() {
    }
}
