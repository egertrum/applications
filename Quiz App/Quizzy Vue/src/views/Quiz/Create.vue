<template>
    <div>
        <h1 v-if="!id" class="text-center">Create new Quiz</h1>
        <h1 v-else class="text-center">{{ quiz.name }}</h1>

        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="control-label">Quiz name</label>
                    <input
                        class="form-control"
                        type="text"
                        maxlength="128"
                        name="Title"
                        v-model="quiz.name"
                    />
                    <span class="errorClass" v-if="errors.name">{{errors.name}}</span>
                </div>
                <div class="form-group">
                    <button
                        v-on:click="this.createQuiz()"
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
import { IQuiz } from "@/domain/IQuiz";
import { QuizErrors } from "@/domain/errors/QuizErrors";
import Loader from "@/components/Loader.vue";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import { IFetchResponse } from "@/types/IFetchResponse";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuizCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id?: string;
    errors: QuizErrors = new QuizErrors();
    buttonText: string = "Create";
    quiz: IQuiz = {
        name: ""
    };

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }
        if(this.id) {
            this.buttonText = "Edit";
            const quizResult = await BaseService.get<IQuiz>("/Quiz/" + this.id, store.state.token as string);
            if (quizResult.ok) {
                this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
                this.quiz = quizResult.data as IQuiz;
            } else {
                this.pageLoader = {
                    pageStatus: EPageStatus.Error,
                    statusCode: quizResult.statusCode
                };
            }
        }
    }

    handleValidation(): boolean {
        let formIsValid = true;

        if (!this.quiz.name || this.quiz.name === "") {
            this.errors.name = "Quiz name can not be empty.";
            formIsValid = false;
        }

        return formIsValid;
    }

    async createQuiz(): Promise<void> {

        if (!this.handleValidation()) {
            return;
        }

        let result: IFetchResponse<IQuiz>;

        if (!this.id) {
            result = await BaseService.post<IQuiz>("/Quiz", this.quiz, store.state.token as string);
        } else {
            result = await BaseService.put<IQuiz>("/Quiz/" + this.id, this.quiz, store.state.token as string);
        }

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/");
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>