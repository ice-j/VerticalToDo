import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import { AuthenticationService } from "@/Services/AuthenticationService";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    redirect: () =>
      AuthenticationService.isLoggedIn() ? "/tasks/list" : "/accounts/login"
  },
  {
    path: "/tasks",
    component: () => import("../views/layouts/DefaultLayout.vue"),
    beforeEnter: (to, from, next) => {
      let auth = AuthenticationService.isLoggedIn();
      console.log("beforeEnter auth: " + auth);
      if (auth != true) next("/accounts/login?redirectTo=" + to.path);
      else next();
    },
    children: [
      {
        path: "",
        redirect: "list"
      },
      {
        path: "list",
        name: "List",
        component: () => import("../views/ToDoItems/List.vue")
      },
      {
        path: "new",
        name: "New",
        component: () => import("../views/ToDoItems/New.vue")
      },
      {
        path: "edit/:id",
        name: "Edit",
        beforeEnter: (to, from, next) => {
          console.log(to.params);
          if (to.params.id) next();
          else next(false);
        },
        component: () => import("../views/ToDoItems/Edit.vue")
      },
      {
        path: "archive",
        name: "Archive",
        component: () => import("../views/ToDoItems/Archive.vue")
      },
      {
        path: "*",
        component: () => import("../views/NotFound.vue")
      }
    ]
  },
  {
    path: "/accounts*",
    component: () => import("../views/layouts/AuthLayout.vue"),
    children: [
      {
        path: "login:redirectTo?",
        name: "Login",
        component: () => import("../views/Accounts/Login.vue")
      },
      {
        path: "register",
        name: "Register",
        component: () => import("../views/Accounts/Register.vue")
      }
    ]
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
