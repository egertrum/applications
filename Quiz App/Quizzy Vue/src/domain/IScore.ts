import { IQuiz } from "./IQuiz";

export interface IScore {
    id?: string | null,
    quiz?: IQuiz,
    quizId: string,
    amount: string,
    passed: boolean
}