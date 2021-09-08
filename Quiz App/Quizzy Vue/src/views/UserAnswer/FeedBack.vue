<template>
    <div v-if="feedback">
        <h1 class="text-center">Results</h1>

        <h4 class="text-center">Points: {{feedback.correctAnswers}}/{{feedback.maxPoints}}</h4>

        <table class="table table-hover">
            <thead>
                <tr>
                    <th>Question</th>
                    <th class="text-center">Answer</th>
                    <th class="text-center">Correct</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in feedback.userAnswers"
                    :key="item.id"
                    :value="item.id"
                    >
                    <td>{{item.questionAnswer.question.value}}</td>
                    <td class="text-center">{{item.questionAnswer.value}}</td>
                    <td class="text-center" v-if="item.questionAnswer.true">
                        <img
                            class="extra-small-icon"
                            src="@/assets/images/tick.png"
                            alt=""
                        />
                    </td>
                    <td class="text-center" v-else>
                        <img
                            class="extra-small-icon"
                            src="@/assets/images/wrong.png"
                            alt=""
                        />
                    </td>
                </tr>
            </tbody>
        </table>

        <div>
            <router-link class="btn btn-primary" to="/"
                >Back to Home</router-link
            >
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
import { IUserAnswerFeedback } from "@/domain/IUserAnswerFeedback";

@Options({
    components: {
        Loader,
    },
    props: {
        uniqueId: String,
        quizId: String,
    },
})
export default class FeedBack extends Vue {
    uniqueId?: string;
    quizId?: string;
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };

    maxNum: number = 0;

    feedback: IUserAnswerFeedback | null = null;

    get isUserLoggedAdmin(): boolean {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        const questionAnswerUrl = "/UserAnswer/Feedback?uniqueId=" + this.uniqueId + "&quizId=" + this.quizId;
        const resultQuestion = await BaseService.get<IUserAnswerFeedback>(questionAnswerUrl);

        if (resultQuestion.ok && resultQuestion.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.feedback = resultQuestion.data;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultQuestion.statusCode,
            };
        }
    }
}
</script>
