<template>
    <div v-if="pollQuestion && pollQuestion.question && pollQuestion.poll">
        <h3 class="text-center">Are you sure you want to delete this question from that quiz?</h3>
        <div>
            <h4 class="text-center">{{pollQuestion.question.value}}</h4>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Poll
                </dt>
                <dd class="col-sm-10">
                    {{pollQuestion.poll.name}}
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
import { IPollQuestion } from "@/domain/IPollQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class PollQuestionDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    pollQuestion: IPollQuestion | null = null;
    pollId: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const quizResult = await BaseService.get<IPollQuestion>("/PollQuestion/" + this.id, store.state.token as string);

        if (quizResult.ok && quizResult.data) {
            console.log(quizResult);
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.pollQuestion = quizResult.data as IPollQuestion;
            this.pollId = quizResult.data.pollId;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const quizResult = await BaseService.delete<IPollQuestion>("/PollQuestion/" + this.id, store.state.token as string);
        if (quizResult.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Poll/Info/" + this.pollId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: quizResult.statusCode
            };
        }
    }
}
</script>
