<template>
    <div v-if="quiz && quizQuestions">
        <h1 class="text-center">{{ quiz.name }}</h1>

        <table class="table table-hover">
            <thead>
                <tr class="text-center">
                    <th>Question number</th>
                    <th>Question</th>
                    <th></th>
                    <th></th>

                </tr>
            </thead>
            <tbody>
                <tr v-for="item in quizQuestions" :key="item.id">
                    <td class="text-center">{{ item.number }}</td>
                    <td class="text-center">{{ item.question.value }}</td>
                    <td class="text-center">
                    <router-link class="text-dark" :to="'/Question/QuestionCre/' + item.questionId">
                        Edit
                    </router-link>
                    |
                    <router-link class="text-dark" :to='"/QuestionAnswer/QueAnsCre/" + item.questionId' >
                        Add Answer
                    </router-link>
                    |
                    <router-link class="text-dark" :to="'/QuizQuestion/Delete/' + item.id">
                        Remove from quiz
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
import { IQuiz } from "@/domain/IQuiz";
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
export default class QuizInfo extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    quiz: IQuiz | null = null;
    quizQuestions: IQuizQuestion[] | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const quizResult = await BaseService.get<IQuiz>("/Quiz/" + this.id, store.state.token as string);
        const quizQuestionsResult = await BaseService.getAll<IQuizQuestion>("/QuizQuestion/Quiz?quizId=" + this.id, store.state.token as string);

        if (quizResult.ok && quizResult.data && quizQuestionsResult.ok && quizQuestionsResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.quiz = quizResult.data;
            this.quizQuestions = quizQuestionsResult.data as IQuizQuestion[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.ok 
                ? quizQuestionsResult.statusCode
                : quizResult.statusCode,
            };
        }
    }
}
</script>
