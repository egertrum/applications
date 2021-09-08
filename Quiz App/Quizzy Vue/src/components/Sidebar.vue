<template>
    <ul
        class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion"
        id="accordionSidebar"
    >
        <!-- Sidebar - Brand -->
        <router-link
            class="sidebar-brand d-flex align-items-center justify-content-center"
            to="/"
        >
            <div class="sidebar-brand-icon">
                <img class="small-icon" src="@\assets\images\exam.png" alt="" />
            </div>
            <div class="sidebar-brand-text mx-3">Quizzzy <sup></sup></div>
        </router-link>

        <!-- Divider -->
        <hr class="sidebar-divider my-0" />

        <div class="sidebar-heading">Take A Quiz</div>

        <!-- Nav Item - Charts -->
        <li class="nav-item">
            <i class="fas fa-fw fa-chart-area"></i>
            <router-link to="/" class="nav-link">Quizzes</router-link>
        </li>

        <!-- Divider -->
        <hr class="sidebar-divider" />

        <div class="sidebar-heading">Feedback</div>

        <li class="nav-item">
            <i class="fas fa-fw fa-chart-area"></i>
            <router-link to="/Poll" class="nav-link">Polls</router-link>
        </li>

        <!-- Divider -->
        <hr class="sidebar-divider" />
    </ul>
</template>
<script lang="ts">
import { Options, Vue } from "vue-class-component";

@Options({
    components: {},
    props: {},
})
export default class Sidebar extends Vue {}
</script>
