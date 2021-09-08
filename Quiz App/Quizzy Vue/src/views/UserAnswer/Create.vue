<template>
    <div v-if="question && answers">
        <h4 class="text-center" v-if="question">{{question.value}}</h4>
        <hr />
        <div class="row">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <div v-for="item in answers" :key="item.id" :value="item.id">
                    <div class="form-check">
                       <input v-model="answer.questionAnswerId" class="form-check-input" type="radio" name="answerRadio" :id="item.id" :value="item.id">
                        <label class="form-check-label" :for="item.id">
                            {{ item.value }}
                        </label> 
                    </div>
                    <hr/>
                </div>
                <div class="form-group text-center">
                    <button v-on:click="this.addAnswer()" class="btn btn-primary">{{ buttonText }}</button>
                </div>
            </div>
            <div class="col-md-4"></div>
        </div>
    </div>
    <Loader :pageLoader="pageLoader" />
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import store from "../../store/index";
import { BaseService } from "../../services/base-service";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import Loader from "@/components/Loader.vue";
import { IQuestion } from "@/domain/IQuestion";
import { IQuestionAnswer } from "@/domain/IQuestionAnswer";
import { IUserAnswer } from "@/domain/IUserAnswer";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String,
        type: String,
        uniqueId: String || undefined
    },
})
export default class UserAnswerCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    type!: string;
    uniqueId?: string;

    question: IQuestion | null = null;
    answers: IQuestionAnswer[] | null = null;

    maxNum?: number;
    questionNum = 1;
    unique: string = "";
    answerUnique: string = "";

    buttonText: string = "Next Question";

    answer: IUserAnswer = {
        quizId: this.id,
        questionAnswerId: ""
    };

    get isUserLoggedAdmin(): boolean {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        await this.loadData();
    }

    async loadData(): Promise<void> {

        if(this.maxNum == this.questionNum) {
            this.buttonText = "Submit";
        }

        const questionUrl = this.type == "quiz" 
        ? "/QuizQuestion/Quiz/Number?quizId=" + this.id + "&number=" + this.questionNum
        : "/PollQuestion/Poll/Number?pollId=" + this.id + "&number=" + this.questionNum;
        const resultQuestion = await BaseService.get<IQuestion>(questionUrl);

        if (this.questionNum == 1) {
            const maxQustionsUrl = this.type == "quiz" ? "/QuizQuestion/Max?quizId=" + this.id : "/PollQuestion/Max?pollId=" + this.id; 
            const questionNumber = await BaseService.get<number>(maxQustionsUrl);
            if (questionNumber.ok && questionNumber.data) {
                this.maxNum = questionNumber.data;
            } else {
                this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: questionNumber.statusCode
                };
            }
        }

        if ( resultQuestion.ok && resultQuestion.data ) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.question = resultQuestion.data;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultQuestion.statusCode
            };
        }

        const answersUrl = "/QuestionAnswers?id=" + this.question!.id;
        const resultAnswers = await BaseService.getAll<IQuestionAnswer>(answersUrl);
        if ( resultAnswers.ok && resultAnswers.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.answers = resultAnswers.data as IQuestionAnswer[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultAnswers.statusCode
            };
        }
    }

    async addAnswer(): Promise<void> {
        if (!this.answer.questionAnswerId || (this.answerUnique == this.answer.questionAnswerId)) {
            return;
        }
        if (this.unique) {
            this.answer.uniqueQuizId = this.unique;
        }
        if (this.type == "poll") {
            this.answer.quizId = undefined;
        }
        const answerUrl = "/UserAnswer";
        const result = await BaseService.post<IUserAnswer>(answerUrl, this.answer);

        if (result.ok && result.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            if (this.maxNum! <= this.questionNum) {
                if (this.type == "quiz") {
                    this.$router.push("/UserAnswer/feedback/" + this.unique + "/" + this.id);
                } else {
                    this.$router.push("/Poll/d");
                }
                return;
            }
            this.answerUnique = result.data.questionAnswerId!;
            this.unique = result.data.uniqueQuizId!;
            this.questionNum++;
            this.loadData();
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>
