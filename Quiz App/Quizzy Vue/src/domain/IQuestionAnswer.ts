import { IQuestion } from "./IQuestion";

export interface IQuestionAnswer {
    id?: string | null,
    questionId: string,
    question?: IQuestion,
    value: string,
    true: boolean
}