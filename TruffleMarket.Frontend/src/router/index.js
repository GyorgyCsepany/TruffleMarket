import { createRouter, createWebHistory } from "vue-router";
import Items from "../views/Items.vue";
import NewOffer from "../views/NewOffer.vue";
import UserPage from "../views/UserPage.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: "Items",
      path: "/",
      component: Items,
      meta: {
        auth: false,
      },
    },
    {
      name: "NewOffer",
      path: "/user/:userId?/offer",
      component: NewOffer,
      meta: {
        auth: true,
      },
    },
    {
      name: "UserBids",
      path: "/user/:userId?/bids",
      component: UserBids,
      meta: {
        auth: true,
      },
    },
    {
      name: "LoginOrRegister",
      path: "/login",
      component: UserPage,
      meta: {
        auth: false,
      },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const user = localStorage.user;
  if (to.matched.some((res) => res.meta.auth)) {
    if (user) {
      next();
      return;
    }
    next({ name: "LoginOrRegister" });
    document.getElementsByClassName("el-menu-item")[4].click();
    return;
  }
  next();
});

export default router;
