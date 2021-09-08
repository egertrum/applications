import { IUserAnswer } from "./IUserAnswer";

export interface IUserAnswerFeedback {
    userAnswers: IUserAnswer[],
    correctAnswers: number,
    maxPoints: number
}