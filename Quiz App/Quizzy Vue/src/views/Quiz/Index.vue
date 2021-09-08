<template>
    <h1 class="text-center">Quizzes</h1>
    <p class="text-center" v-if="isAdmin">
        <router-link class="text-dark" to="/QuizCre/">
            Add new Quiz
            <img class="extra-small-icon" src="@\assets\images\right-arrow.png" alt="">
        </router-link>
    </p>

    <table class="table table-hover">
        <thead>
            <tr class="text-center">
                <th>Question</th>
                <th>Average result</th>
                <th>Passing percent (Answerers who got more than 50% right)</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in quizzes" :key="item.id" class="text-center">
                <td>{{ item.name }}</td>
                <td v-if="item.average && item.maxPoints">{{ item.average }} / {{ item.maxPoints }}</td>
                <td v-else></td>
                <td v-if="item.passingProc">{{ item.passingProc }}%</td>
                <td v-else></td>
                <td v-if="!isAdmin">
                <router-link class="btn btn-info" :to="'/UserAnswer/create/' + item.id + '/quiz/'">
                    Take Quiz
                </router-link>
                </td>
                <td v-else>
                <router-link class="text-dark" :to="'/Info/' + item.id">
                    Info
                </router-link>
                |
                <router-link class="text-dark" :to="'/QuizCre/' + item.id">
                    Edit
                </router-link>
                |
                <router-link class="text-dark" :to="'/QuizQuestion/Create/' + item.id">
                    Add Question
                </router-link>
                |
                <router-link class="text-dark" :to="'/QuizDel/' + item.id">
                    Delete
                </router-link>
                </td>
            </tr>
        </tbody>
    </table>
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
    },
})
export default class QuizIndex extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    quizzes: IQuiz[] | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        const resultQuizzes = await BaseService.getAll<IQuiz>("/Quiz", store.state.token as string);

        if (resultQuizzes.ok && resultQuizzes.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.quizzes = resultQuizzes.data as IQuiz[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultQuizzes.statusCode,
            };
        }
    }
}
</script>
