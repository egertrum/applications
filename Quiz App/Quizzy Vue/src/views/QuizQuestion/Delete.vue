<template>
    <div v-if="quizQuestion && quizQuestion.question && quizQuestion.quiz">
        <h3 class="text-center">Are you sure you want to delete this question from that quiz?</h3>
        <div>
            <h4 class="text-center">{{quizQuestion.question.value}}</h4>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Quiz
                </dt>
                <dd class="col-sm-10">
                    {{quizQuestion.quiz.name}}
                </dd>
            </dl>
            <div class="form-group">
                <button
                    v-on:click="this.deleteClicked()"
                    class="btn btn-danger"
                >
                    Delete
                </button>
            </div>
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
import { IQuizQuestion } from "@/domain/IQuizQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuizQuestionDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    quizQuestion: IQuizQuestion | null = null;
    quizId: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const quizResult = await BaseService.get<IQuizQuestion>("/QuizQuestion/" + this.id, store.state.token as string);

        if (quizResult.ok && quizResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.quizQuestion = quizResult.data as IQuizQuestion;
            this.quizId = quizResult.data.quizId;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const quizResult = await BaseService.delete<IQuizQuestion>("/QuizQuestion/" + this.id, store.state.token as string);
        if (quizResult.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Info/" + this.quizId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }
}
</script>
