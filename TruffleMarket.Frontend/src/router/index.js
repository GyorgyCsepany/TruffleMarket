import { createRouter, createWebHistory } from "vue-router";
import Items from "../components/Items.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      component: Items,
    },
  ],
});

export default router;
