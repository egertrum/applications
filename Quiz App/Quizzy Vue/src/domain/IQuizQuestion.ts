import { IQuestion } from "./IQuestion";
import { IQuiz } from "./IQuiz";

export interface IQuizQuestion {
    id?: string | null,
    quizId: string,
    quiz?: IQuiz,
    questionId: string,
    question?: IQuestion | null,
    number?: string | null
}