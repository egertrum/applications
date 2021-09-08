<template>
    <div v-if="!isAdmin">
        <h1 class="text-center">Leave us feedback</h1>
        <h4 class="text-center">Answer to polls</h4>
        <p v-if="done" class="alert-success text-center">Thank you for your feedback!</p>
    </div>
    <div v-else>
        <h1 class="text-center">Polls</h1>
        <p class="text-center">
        <router-link class="text-dark" to="/Poll/PollCre/">
             Add new Poll
            <img class="extra-small-icon" src="@\assets\images\right-arrow.png" alt="">
        </router-link>
    </p>
    </div>

    <table class="table table-hover">
        <tbody class="text-center">
            <tr v-for="item in polls" :key="item.id">
                <td>{{ item.name }}</td>
                <td v-if="!isAdmin">
                <router-link class="btn btn-info" :to="'/UserAnswer/create/' + item.id + '/poll/'">
                    Leave feedback
                </router-link>
                </td>
                <td v-else>
                <router-link class="text-dark" :to="'/Poll/Info/' + item.id">
                    Info
                </router-link>
                |
                <router-link class="text-dark" :to="'/Poll/PollCre/' + item.id">
                    Edit
                </router-link>
                |
                <router-link class="text-dark" :to="'/PollQuestion/Create/' + item.id">
                    Add Question
                </router-link>
                |
                <router-link class="text-dark" :to="'/Poll/PollDel/' + item.id">
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
import { IPoll } from "@/domain/IPoll";

@Options({
    components: {
        Loader,
    },
    props: {
        done: String
    },
})

export default class PollIndex extends Vue {
    pageLoader: PageLoader = {
        pageStatus: EPageStatus.Loading,
        statusCode: -1,
    };
    done?: string;
    polls: IPoll[] | null = null;

    get isAdmin(): boolean {
        return store.state.role === "Admin";
    }

    async mounted(): Promise<void> {
        const resultPolls = await BaseService.getAll<IPoll>("/Poll", store.state.token as string);

        if (resultPolls.ok && resultPolls.data) {
            this.pageLoader = { pageStatus: EPageStatus.OK, statusCode: 0 };
            this.polls = resultPolls.data as IPoll[];
        } else {
            this.pageLoader = {
                pageStatus: EPageStatus.Error,
                statusCode: resultPolls.statusCode,
            };
        }
    }
}
</script>
