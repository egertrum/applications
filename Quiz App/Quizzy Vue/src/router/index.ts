import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Login from '../views/Identity/Login.vue'
import Register from '../views/Identity/Register.vue'

import QuizIndex from '../views/Quiz/Index.vue'
import QuizDelete from '../views/Quiz/Delete.vue'
import QuizInfo from '../views/Quiz/Info.vue'
import QuizCreate from '../views/Quiz/Create.vue'

import PollIndex from '../views/Poll/Index.vue'
import PollInfo from '../views/Poll/Info.vue'
import PollCreate from '../views/Poll/Create.vue'
import PollDelete from '../views/Poll/Delete.vue'

import QuestionIndex from '../views/Question/Index.vue'
import QuestionCreate from '../views/Question/Create.vue'
import QuestionDelete from '../views/Question/Delete.vue'

import QuestionAnswerIndex from '../views/QuestionAnswer/Index.vue'
import QuestionAnswerCreate from '../views/QuestionAnswer/Create.vue'
import QuestionAnswerDelete from '../views/QuestionAnswer/Delete.vue'

import QuizQuestionCreate from '../views/QuizQuestion/Create.vue'
import QuizQuestionDelete from '../views/QuizQuestion/Delete.vue'

import PollQuestionCreate from '../views/PollQuestion/Create.vue'
import PollQuestionDelete from '../views/PollQuestion/Delete.vue'

import UserAnswerCreate from '../views/UserAnswer/Create.vue'
import FeedBack from '../views/UserAnswer/FeedBack.vue'

const routes: Array<RouteRecordRaw> = [
  { path: '/Login', name: 'Login', component: Login },
  { path: '/Register', name: 'Register', component: Register },

  { path: '/Poll/:done?', name: 'Polls', component: PollIndex, props: true },
  { path: '/Poll/PollCre/:id?', name: 'PollCreate', component: PollCreate, props: true },
  { path: '/Poll/PollDel/:id?', name: 'PollDelete', component: PollDelete, props: true },
  { path: '/Poll/Info/:id', name: 'PollInfo', component: PollInfo, props: true },
  
  { path: '/UserAnswer/create/:id/:type/:uniqueId?', name: 'UserAnswers', component: UserAnswerCreate, props: true },
  { path: '/', name: 'Quizzes', component: QuizIndex },
  { path: '/QuizCre/:id?', name: 'QuizCreate', component: QuizCreate, props: true },
  { path: '/QuizDel/:id?', name: 'QuizDelete', component: QuizDelete, props: true },
  { path: '/Info/:id', name: 'QuizInfo', component: QuizInfo, props: true },

  { path: '/Question', name: 'Questions', component: QuestionIndex, props: true },
  { path: '/Question/QuestionCre/:id?', name: 'QuestionCreate', component: QuestionCreate, props: true },
  { path: '/Question/QuestionDel/:id?', name: 'QuestionDelete', component: QuestionDelete, props: true },

  { path: '/QuestionAnswer/:id', name: 'QuestionAnswers', component: QuestionAnswerIndex, props: true },
  { path: '/QuestionAnswer/DelQueAns/:id', name: 'QuestionAnswerDelete', component: QuestionAnswerDelete, props: true },
  { path: '/QuestionAnswer/QueAnsCre/:questionId/:id?', name: 'QuestionAnswerCreate', component: QuestionAnswerCreate, props: true },

  { path: '/QuizQuestion/Create/:quizId?', name: 'QuizQuestionCreate', component: QuizQuestionCreate, props: true },
  { path: '/QuizQuestion/Delete/:id', name: 'QuizQuestionDelete', component: QuizQuestionDelete, props: true },

  { path: '/PollQuestion/Create/:pollId', name: 'PollQuestionCreate', component: PollQuestionCreate, props: true },
  { path: '/PollQuestion/Delete/:id', name: 'PollQuestionDelete', component: PollQuestionDelete, props: true },
  
  { path: '/UserAnswer/feedback/:uniqueId/:quizId', name: 'FeedBack', component: FeedBack, props: true},
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
