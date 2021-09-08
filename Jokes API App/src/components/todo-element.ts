import { ITodo } from './../domain/ITodo';
import { bindable } from "@aurelia/runtime-html";

export class TodoElement {

    @bindable public item: ITodo;
    
    @bindable public removeCallback : (index: number) => void = null;

    @bindable todoNo: number;

    removeTodo(index: number){
        this.removeCallback(index);
    }
}
