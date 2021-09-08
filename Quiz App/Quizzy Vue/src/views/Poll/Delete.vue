<template>
    <div v-if="poll">
        <h3 class="text-center">Are you sure you want to delete this poll?</h3>
        <div>
            <h4 class="text-center">{{poll.name}}</h4>
            <p v-if="error" class="text-center errorClass">{{error}}</p>
            <hr>
            <dl class="row">
                <dt class="col-sm-2">
                    Name
                </dt>
                <dd class="col-sm-10">
                    {{poll.name}}
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
import { IPoll } from "@/domain/IPoll";

@Options({
    components: {
        Loader,
    },
    props: {
        id: String
    },
})
export default class PollDelete extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    id!: string;
    poll: IPoll | null = null;
    error: string | null = null;

    get isAdmin(): boolean | null {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        if(!this.isAdmin){
            this.$router.push("/Login");
        }
        const pollResult = await BaseService.get<IPoll>("/Poll/" + this.id, store.state.token as string);

        if (pollResult.ok && pollResult.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.poll = pollResult.data;
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: pollResult.statusCode
            };
        }
    }

    async deleteClicked(): Promise<void> {
        const pollResult = await BaseService.delete<IPoll>("/Poll/" + this.id, store.state.token as string);
        if (pollResult.ok) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.$router.push("/Poll");
        } else if (pollResult.statusCode == 400) {
            this.error = "Can not delete unless you try to delete all of the sequences of this poll questions.";
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: pollResult.statusCode
            };
        }
    }
}
</script>
