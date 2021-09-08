<template>
    <div v-if="questions && poll">
        <h1 class="text-center">Add new question</h1>
        <h4 class="text-center">{{poll.name}}</h4>

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
                            v-model="pollQuestion.questionId"
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
                        v-on:click="this.createPollQuestion()"
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
import { IPoll } from "@/domain/IPoll";
import { IPollQuestion } from "@/domain/IPollQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        pollId: String
    },
})
export default class PollQuestionCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    pollId!: string;
    poll: IPoll | null = null;

    errors: QuizQuestionErrors = new QuizQuestionErrors();
    questions: IQuestion[] | null = null;
    pollQuestion: IPollQuestion = {
        pollId: this.pollId,
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
        let quizRes = await BaseService.get<IPoll>("/Poll/" + this.pollId, store.state.token as string);

        if (result.ok && result.data && quizRes.ok && quizRes.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.questions = result.data as IQuestion[];
            this.poll = quizRes.data as IPoll;
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

        if (!this.pollQuestion.questionId || this.pollQuestion.questionId === "") {
            this.errors.questionId = "Question value can not be empty.";
            formIsValid = false;
        }

        return formIsValid;
    }

    async createPollQuestion(): Promise<void> {
        if (!this.handleValidation()) {
            return;
        }

        let result = await BaseService.post<IPollQuestion>("/PollQuestion", this.pollQuestion, store.state.token as string);

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Poll/Info/" + this.pollId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>