<template>
    <div v-if="quiz">
        <h3 class="text-center">Are you sure you want to delete this?</h3>
        <div>
            <h4 class="text-center">{{quiz.name}}</h4>
            <p v-if="error" class="text-center errorClass">{{error}}</p>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Name
                </dt>
                <dd class="col-sm-10">
                    {{quiz.name}}
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
import { IQuiz } from "@/domain/IQuiz";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import Loader from "@/components/Loader.vue";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuizDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    quiz: IQuiz | null = null;
    error: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const quizResult = await BaseService.get<IQuiz>("/Quiz/" + this.id, store.state.token as string);

        if (quizResult.ok && quizResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.quiz = quizResult.data;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const quizResult = await BaseService.delete<IQuiz>("/Quiz/" + this.id, store.state.token as string);
        if (quizResult.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/");
        } else if (quizResult.statusCode == 400) {
            this.error = "Can not delete unless you try to delete all of the sequences of this quiz questions.";
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }
}
</script>
