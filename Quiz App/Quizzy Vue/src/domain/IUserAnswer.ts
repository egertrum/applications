import { IQuestionAnswer } from "./IQuestionAnswer";
import { IQuiz } from "./IQuiz";

export interface IUserAnswer {
    id?: string | null,
    quiz?: IQuiz,
    quizId?: string,
    uniqueQuizId?: string,
    questionAnswerId: string
    questionAnswer?: IQuestionAnswer
}