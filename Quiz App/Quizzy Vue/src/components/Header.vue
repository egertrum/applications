<template>
    <header
        class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow"
    >
        <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target=".navbar-collapse"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
        >
            <span class="navbar-toggler-icon"></span>
        </button>

        <ul v-if="token == null" class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link text-dark" href="/Identity/Account/Register"
                    >Register</a
                >
            </li>
            <li class="nav-item">
                <router-link to="/Login" class="nav-link text-dark"
                    >Login</router-link
                >
            </li>
        </ul>
        <ul v-if="token != null" class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link text-dark" href="/Identity/Account/Register"
                    >Hello, {{ name }}!</a
                >
            </li>
            <li class="nav-item">
                <a href="#" class="nav-link text-dark" @click="logOut()">
                    Logout</a
                >
            </li>
        </ul>

        <ul v-if="role == 'Admin'" class="navbar-nav">
            <div class="topbar-divider d-none d-sm-block"></div>
            <li class="nav-item">
                <router-link class="nav-link text-dark" to="/Question"
                    >Questions</router-link
                >
            </li>
            <div class="topbar-divider d-none d-sm-block"></div>
            <li class="nav-item">
                <router-link class="nav-link text-dark" to="/"
                    >Quizzes</router-link
                >
            </li>
            <div class="topbar-divider d-none d-sm-block"></div>
            <li class="nav-item">
                <router-link class="nav-link text-dark" to="/Poll"
                    >Polls</router-link
                >
            </li>
        </ul>

    </header>
</template>
<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import store from '@/store/index'

@Options({
    components: {},
    props: {}
})
export default class Header extends Vue {
    get token(): string | null {
        return store.state.token;
    }
    get role(): string | null {
        return store.state.role;
    }
    get name(): string | null {
        return store.state.firstname + " " + store.state.lastname
    }

    logOut(): void {
        store.commit("logOut");
        this.$router.push("/");
    }
}
</script>
