<template>
    <div v-if="answer && answer.question">
        <h3 class="text-center">Are you sure you want to delete this answer from that question?</h3>
        <div>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Question
                </dt>
                <dd class="col-sm-10">
                    {{answer.question.value}}
                </dd>
                <dt class="col-sm-2">
                    Value
                </dt>
                <dd class="col-sm-10">
                    {{answer.value}}
                </dd>
                <dt class="col-sm-2">
                    Correct
                </dt>
                <dd v-if="answer.true">
                    <input checked="checked" class="check-box" disabled="disabled" type="checkbox">
                </dd>
                <dd v-else>
                    <input class="check-box" disabled="disabled" type="checkbox">
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
import { IQuestionAnswer } from "@/domain/IQuestionAnswer";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuestionAnswerDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    answer: IQuestionAnswer | null = null;
    questionId: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const result = await BaseService.get<IQuestionAnswer>("/QuestionAnswer/" + this.id, store.state.token as string);

        if (result.ok && result.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.answer = result.data as IQuestionAnswer;
            this.questionId = result.data.questionId;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const result = await BaseService.delete<IQuestionAnswer>("/QuestionAnswer/" + this.id, store.state.token as string);
        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/QuestionAnswer/" + this.questionId);
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>
