
<template>
    <body id="page-top">
        <div id="wrapper">
            <Sidebar />

            <div id="content-wrapper" class="d-flex flex-column">
                <Header />

                <div class="container-fluid">
                    <div id="content">
                        <div class="container">
                            <main role="main" class="pb-3">
                                <router-view />
                            </main>
                        </div>
                    </div>
                </div>

                <Footer />
            </div>
        </div>
    </body>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import store from "./store/index";
import Sidebar from "@/components/Sidebar.vue";
import Header from "@/components/Header.vue";
import Footer from "@/components/Footer.vue";
import router from "vue-router";

@Options({
    // Do not forget to register imported components!!!!!
    components: {
        Sidebar,
        Header,
        Footer,
    },
})
export default class App extends Vue {
    get token(): string | null {
        return store.state.token;
    }

    get role(): string | null {
        return store.state.firstname;
    }

    logOut(): void {
        store.commit("logOut");
        this.$router.push("/");
    }
}
</script>
