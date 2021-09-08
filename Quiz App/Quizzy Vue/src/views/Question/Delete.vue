<template>
    <div v-if="question">
        <h3 class="text-center">Are you sure you want to delete this question?</h3>
        <div>
            <p v-if="error" class="text-center errorClass">{{error}}</p>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Value
                </dt>
                <dd class="col-sm-10">
                    {{question.value}}
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
import { IQuestion } from "@/domain/IQuestion";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class QuestionDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    question: IQuestion | null = null;
    error: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const questionResult = await BaseService.get<IQuestion>("/Question/" + this.id, store.state.token as string);

        if (questionResult.ok && questionResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.question = questionResult.data;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: questionResult.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const questionResult = await BaseService.delete<IQuestion>("/Question/" + this.id, store.state.token as string);
        if (questionResult.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Question");
        } else if (questionResult.statusCode == 400) {
            this.error = "Can not delete unless you try to delete all of the sequences of this question at Quizzes, Polls and Answers.";
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: questionResult.statusCode
            };
        }
    }
}
</script>
