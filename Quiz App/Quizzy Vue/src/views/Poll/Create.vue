<template>
    <div>
        <h1 v-if="!id" class="text-center">Create new Poll</h1>
        <h1 v-else class="text-center">{{ poll.name }}</h1>

        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="control-label">Poll name</label>
                    <input
                        class="form-control"
                        type="text"
                        maxlength="128"
                        name="Title"
                        v-model="poll.name"
                    />
                    <span class="errorClass" v-if="errors.name">{{errors.name}}</span>
                </div>
                <div class="form-group">
                    <button
                        v-on:click="this.createPoll()"
                        class="btn btn-primary"
                    >
                        {{this.buttonText}}
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import store from "../../store/index";
import { BaseService } from "../../services/base-service";
import { QuizErrors } from "@/domain/errors/QuizErrors";
import Loader from "@/components/Loader.vue";
import { PageLoader } from "@/types/PageLoader";
import { EPageStatus } from "@/types/EPageStatus";
import { IFetchResponse } from "@/types/IFetchResponse";
import { IPoll } from "@/domain/IPoll";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class PollCreate extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id?: string;
    errors: QuizErrors = new QuizErrors();
    buttonText: string = "Create";
    poll: IPoll = {
        name: ""
    };

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if (!this.isAdmin) {
            this.$router.push("/Login");
        }
        if(this.id) {
            this.buttonText = "Edit";
            const pollResult = await BaseService.get<IPoll>("/Poll/" + this.id, store.state.token as string);
            if (pollResult.ok) {
                this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
                this.poll = pollResult.data as IPoll;
            } else {
                this.pageLoader = {
                    pageStatus: EPageStatus.Error,
                    statusCode: pollResult.statusCode
                };
            }
        }
    }

    handleValidation(): boolean {
        let formIsValid = true;

        if (!this.poll.name || this.poll.name === "") {
            this.errors.name = "Poll name can not be empty.";
            formIsValid = false;
        }
        return formIsValid;
    }

    async createPoll(): Promise<void> {
        if (!this.handleValidation()) {
            return;
        }
        let result: IFetchResponse<IPoll>;

        if (!this.id) {
            result = await BaseService.post<IPoll>("/Poll", this.poll, store.state.token as string);
        } else {
            result = await BaseService.put<IPoll>("/Poll/" + this.id, this.poll, store.state.token as string);
        }

        if (result.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Poll");
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: result.statusCode
            };
        }
    }
}
</script>