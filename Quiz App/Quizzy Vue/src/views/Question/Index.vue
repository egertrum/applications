<template>
    <h1 class="text-center">Questions</h1>
    <p class="text-center" v-if="isAdmin">
        <router-link class="text-dark" to="/Question/QuestionCre/">
            Add new Question
            <img class="extra-small-icon" src="@\assets\images\right-arrow.png" alt="">
        </router-link>
    </p>

    <table class="table table-hover">
        <thead>
            <tr>
                <th>Question</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in questions" :key="item.id">
                <td>{{ item.value }}</td>
                <td>
                <router-link class="text-dark" :to="'/QuestionAnswer/' + item.id">
                    Answers
                </router-link>
                |
                <router-link class="text-dark" :to="'/Question/QuestionCre/' + item.id">
                    Edit
                </router-link>
                |
                <router-link class="text-dark" :to="'/QuestionAnswer/QueAnsCre/' + item.id">
                    Add Answer
                </router-link>
                |
                <router-link class="text-dark" :to="'/Question/QuestionDel/' + item.id">
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
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import Loader from "@/components/Loader.vue";
import { IQuestion } from "@/domain/IQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
    },
})
export default class QuestionIndex extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    questions: IQuestion[] | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }

        const resultQue = await BaseService.getAll<IQuestion>("/Question", store.state.token as string);

        if (resultQue.ok && resultQue.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.questions = resultQue.data as IQuestion[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultQue.statusCode,
            };
        }
    }
}
</script>
