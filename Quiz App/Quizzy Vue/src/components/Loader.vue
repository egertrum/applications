<template>
    <div v-if="pageLoader.pageStatus === pageStatusLoading" >
        <div className="alert alert-primary" role="alert">Loading...</div>
    </div>
    <div v-else-if="pageLoader.pageStatus === pageStatusError" >
        <div className="alert alert-danger" role="alert">Error... {{ pageLoader.statusCode }}</div>
    </div>
</template>
<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import { EPageStatus } from "../types/EPageStatus";
import { PageLoader } from "../types/PageLoader";

@Options({
    components: {},
    props: {
        pageLoader: {},
    }
})
export default class Loader extends Vue {
    pageLoader!: PageLoader;
    pageStatusLoading: EPageStatus = EPageStatus.Loading;
    pageStatusError: EPageStatus = EPageStatus.Error;
    pageStatusOk: EPageStatus = EPageStatus.OK;
}
</script>
