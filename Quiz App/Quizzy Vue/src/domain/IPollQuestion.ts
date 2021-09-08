import { IPoll } from "./IPoll";
import { IQuestion } from "./IQuestion";

export interface IPollQuestion {
    id?: string | null,
    pollId: string,
    poll?: IPoll,
    questionId: string,
    question?: IQuestion,
    number?: string | null
}