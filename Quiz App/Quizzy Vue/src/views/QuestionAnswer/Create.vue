<template>
    <div v-if="answer && question">
        <h1 v-if="!id" class="text-center">Add answer</h1>
        <h1 v-else class="text-center">Edit answer</h1>
        <h4 class="text-center">{{question.value}}</h4>

        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="control-label">Answer</label>
                    <input
                        class="form-control"
                        type="text"
                        maxlength="256"
                        name="Title"
                        v-model="answer.value"
                    />
                    <span class="errorClass" v-if="errors.value">{{errors.value}}</span>
                </div>
                <div class="form-check">
                    <input class="form-check-input" v-model="answer.true" type="checkbox" id="defaultCheck1">
                    <label class="form-check-label" for="defaultCheck1">
                        Correct answer
                    </label>
                </div>
                <div class="form-group">
                    <button
                        v-on:click="this.createAnswer()"
                        class="btn btn-primary"
                    >
                        {{buttonText}}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import store from "../../store/index";
import { BaseService } from "../../services/base-service";
import { QuestionAnswerErrors } from "@/domain/errors/QuestionAnswerErrors";
import Loader from "@/components/Loader.vue";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import { IQuestion } from "@/domain/IQuestion";
import { IQuestionAnswer } from "@/domain/IQuestionAnswer";
import { IFetchResponse } from "@/types/IFetchResponse";

@Options({
    components: {
        Loader,
    },
    props: {
        questionId: String,
        id: String
    },
})
export default class QuestionAnswerCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    questionId!: string;
    id?: string;
    errors: QuestionAnswerErrors = new QuestionAnswerErrors();
    question: IQuestion | null = null;
    answer: IQuestionAnswer = {
        value: "",
        true: false,
        questionId: this.questionId
    };
    buttonText: string = "Add";

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }

        let result = await BaseService.get<IQuestion>("/Question/" + this.questionId,  store.state.token as string);

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.question = result.data as IQuestion;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }

        if (this.id) {
            this.buttonText = "Edit";
            let answerRes = await BaseService.get<IQuestionAnswer>("/QuestionAnswer/" + this.id,  store.state.token as string);

            if (answerRes.ok) {
                this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
                this.answer = answerRes.data as IQuestionAnswer;
            } else {
                this.pageLoader = {
                    pageStatus: EPageStatus.Error,
                    statusCode: answerRes.statusCode
                };
            }
        }

    }

    handleValidation(): boolean {
        let formIsValid = true;

        this.errors = new QuestionAnswerErrors();

        if (!this.answer.value || this.answer.value === "") {
            this.errors.value = "Question value can not be empty.";
            formIsValid = false;
        }

        return formIsValid;
    }

    async createAnswer(): Promise<void> {
        if (!this.handleValidation()) {
            return;
        }

        let result: IFetchResponse<IQuestionAnswer>;

        if (!this.id) {
            result = await BaseService.post<IQuestionAnswer>("/QuestionAnswer", this.answer, store.state.token as string);
        } else {
            result = await BaseService.put<IQuestionAnswer>("/QuestionAnswer/" + this.id, this.answer, store.state.token as string);
        }

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/QuestionAnswer/" + this.questionId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>