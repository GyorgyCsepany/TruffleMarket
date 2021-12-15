import { createRouter, createWebHistory } from "vue-router";
import Items from "../views/Items.vue";
import LoginOrRegister from "../views/LoginOrRegister.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: "Items",
      path: "/",
      component: Items,
    },
    {
      name: "LoginOrRegister",
      path: "/login",
      component: LoginOrRegister,
    },
  ],
});

export default router;
