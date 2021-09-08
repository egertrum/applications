<template>
    <div v-if="answers && question">
        <h1 class="text-center">Answers to question</h1>
        <p class="text-center">
            <router-link class="text-dark" :to="'/QuestionAnswer/QueAnsCre/' + question.id">
                Add answer to Question
                <img class="extra-small-icon" src="@\assets\images\right-arrow.png" alt="">
            </router-link>
        </p>

        <table class="table table-hover">
            <thead>
                <tr>
                    <th>Question</th>
                    <th>Answer value</th>
                    <th>Correct answer</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in answers" :key="item.id">
                    <td>{{ item.question.value }}</td>
                    <td>{{ item.value }}</td>
                    <td v-if="item.true">
                        <input checked="checked" class="check-box" disabled="disabled" type="checkbox">
                    </td>
                    <td v-else>
                        <input class="check-box" disabled="disabled" type="checkbox">
                    </td>
                    <td>
                    <router-link class="text-dark" :to="'/QuestionAnswer/QueAnsCre/' + question.id + '/' + item.id">
                        Edit
                    </router-link>
                    |
                    <router-link class="text-dark" :to="'/QuestionAnswer/DelQueAns/' + item.id">
                        Remove Answer
                    </router-link>
                    </td>
                </tr>
            </tbody>
        </table>
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
import { IQuestionAnswer } from "@/domain/IQuestionAnswer";
import { IQuestion } from "@/domain/IQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuestionAnswerIndex extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    answers: IQuestionAnswer[] | null = null;
    question: IQuestion | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }

        const result = await BaseService.getAll<IQuestionAnswer>("/QuestionAnswers/All/?id=" + this.id, store.state.token as string);

        if (result.ok && result.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.answers = result.data as IQuestionAnswer[];

            if (result.data.length <= 0) {
                this.$router.push("/Question");
                return;
            }
            const questionResult = await BaseService.get<IQuestion>("/Question/" + result.data[0].questionId, store.state.token as string);

            if (questionResult.ok && questionResult.data) {
                this.question = questionResult.data as IQuestion;
            } else {
                this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: questionResult.statusCode,
            };
            }

        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode,
            };
        }
    }
}
</script>
