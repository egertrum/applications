<template>
    <div v-if="poll && pollQuestions">
        <h1 class="text-center">{{ poll.name }}</h1>

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
                <tr v-for="item in pollQuestions" :key="item.id">
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
                        <router-link class="text-dark" :to="'/PollQuestion/Delete/' + item.id">
                            Remove from poll
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
import { IPoll } from "@/domain/IPoll";
import { IPollQuestion } from "@/domain/IPollQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class PollInfo extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    poll: IPoll | null = null;
    pollQuestions: IPollQuestion[] | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const pollResult = await BaseService.get<IPoll>("/Poll/" + this.id, store.state.token as string);
        const pollQuestionsResult = await BaseService.getAll<IPollQuestion>("/PollQuestion/Poll?pollId=" + this.id, store.state.token as string);

        if (pollResult.ok && pollResult.data && pollQuestionsResult.ok && pollQuestionsResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.poll = pollResult.data;
            this.pollQuestions = pollQuestionsResult.data as IPollQuestion[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: pollResult.ok 
                ? pollQuestionsResult.statusCode
                : pollResult.statusCode,
            };
        }
    }
}
</script>
