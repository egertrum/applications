<template>
    <div v-if="questions && quiz">
        <h1 class="text-center">Add new question</h1>
        <h4 class="text-center">{{quiz.name}}</h4>

        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <select
                            class="form-control"
                            data-val="true"
                            data-val-required="The In which country is it taking place? field is required."
                            id="Competition_CountryId"
                            name="Competition.CountryId"
                            v-model="quizQuestion.questionId"
                        >
                            <option selected value="">---Please select---</option>
                            <option
                                v-for="item in questions"
                                :key="item.id"
                                :value="item.id"
                            >
                                {{ item.value }}
                            </option>
                        </select>
                    <span class="errorClass" v-if="errors.questionId">{{errors.questionId}}</span>
                </div>
                <div class="form-group">
                    <button
                        v-on:click="this.createAnswer()"
                        class="btn btn-primary"
                    >
                        Add
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
import { QuizQuestionErrors } from "@/domain/errors/QuizQuestionErrors";
import Loader from "@/components/Loader.vue";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import { IQuestion } from "@/domain/IQuestion";
import { IQuizQuestion } from "@/domain/IQuizQuestion";
import { IQuiz } from "@/domain/IQuiz";

@Options({
    components: {
        Loader,
    },
    props: {
        quizId: String
    },
})
export default class QuizQuestionCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    quizId!: string;
    quiz: IQuiz | null = null;

    errors: QuizQuestionErrors = new QuizQuestionErrors();
    questions: IQuestion[] | null = null;
    quizQuestion: IQuizQuestion = {
        quizId: this.quizId,
        questionId: ""
    };

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }

        let result = await BaseService.getAll<IQuestion>("/Question", store.state.token as string);
        let quizRes = await BaseService.get<IQuiz>("/Quiz/" + this.quizId, store.state.token as string);

        if (result.ok && result.data && quizRes.ok && quizRes.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.questions = result.data as IQuestion[];
            this.quiz = quizRes.data as IQuiz;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }

    handleValidation(): boolean {
        let formIsValid = true;

        this.errors = new QuizQuestionErrors();

        if (!this.quizQuestion.questionId || this.quizQuestion.questionId === "") {
            this.errors.questionId = "Question value can not be empty.";
            formIsValid = false;
        }

        return formIsValid;
    }

    async createAnswer(): Promise<void> {
        if (!this.handleValidation()) {
            return;
        }

        let result = await BaseService.post<IQuizQuestion>("/QuizQuestion", this.quizQuestion, store.state.token as string);

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Info/" + this.quizId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>