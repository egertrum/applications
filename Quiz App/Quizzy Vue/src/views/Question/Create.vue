<template>
    <div>
        <h1 v-if="!id" class="text-center">Add new Question</h1>
        <h1 v-else class="text-center">{{ question.value }}</h1>

        <hr />
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="control-label">Question value</label>
                    <input
                        class="form-control"
                        type="text"
                        maxlength="128"
                        name="Title"
                        v-model="question.value"
                    />
                    <span class="errorClass" v-if="errors.value">{{errors.value}}</span>
                </div>
                <div class="form-group">
                        <label class="control-label"
                            >Question type</label
                        >
                        <select
                            class="form-control"
                            id="questionType"
                            name="questionType"
                            v-model="question.questionType"
                        >
                            <option value="">---Please select---</option>
                            <option value="Poll">Poll</option>
                            <option value="Quiz">Quiz</option>
                        </select>
                        <span class="errorClass" v-if="errors.questionType">{{errors.questionType}}</span>
                    </div>
                <div class="form-group">
                    <button
                        v-on:click="this.createQuestion()"
                        class="btn btn-primary"
                    >
                        Create
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
import { QuestionErrors } from "@/domain/errors/QuestionErrors";
import Loader from "@/components/Loader.vue";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import { IQuestion } from "@/domain/IQuestion";
import { contains } from "node_modules/@types/jquery";
import { IFetchResponse } from "@/types/IFetchResponse";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuestionCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id?: string;
    errors: QuestionErrors = new QuestionErrors();
    question: IQuestion = {
        value: "",
        questionType: ""
    };

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }
        if (this.id) {
            let result = await BaseService.get<IQuestion>("/Question/" + this.id, store.state.token as string);

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.question = result.data as IQuestion;
        } else {
                this.pageLoader = {
                    pageStatus: EPageStatus.Error,
                    statusCode: result.statusCode
                };
            }
        }
    }

    handleValidation(): boolean {
        let formIsValid = true;

        this.errors = new QuestionErrors();

        if (!this.question.value || this.question.value === "") {
            this.errors.value = "Question value can not be empty.";
            formIsValid = false;
        }
        if (!this.question.questionType || this.question.questionType === "") {
            this.errors.questionType = "Question type is required.";
            formIsValid = false;
        }

        return formIsValid;
    }

    async createQuestion(): Promise<void> {
        if (!this.handleValidation()) {
            return;
        }

        let result: IFetchResponse<IQuestion>;

        if (!this.id) {
            result = await BaseService.post<IQuestion>("/Question", this.question, store.state.token as string);
        } else {
            result = await BaseService.put<IQuestion>("/Question/" + this.id, this.question, store.state.token as string);
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