import { createRouter, createWebHistory } from "vue-router";
import Items from "../views/Items.vue";
import NewOffer from "../views/NewOffer.vue";
import UserPage from "../views/UserPage.vue";
import UserBids from "../views/UserBids.vue";
import UserOffers from "../views/UserOffers.vue";

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
      path: "/user/newoffer",
      component: NewOffer,
      meta: {
        auth: true,
      },
    },
    {
      name: "UserBids",
      path: "/user/mybids",
      component: UserBids,
      meta: {
        auth: true,
      },
    },
    {
      name: "UserOffers",
      path: "/user/myoffers",
      component: UserOffers,
      meta: {
        auth: true,
      },
    },
    {
      name: "LoginOrRegister",
      path: "/user",
      component: UserPage,
      meta: {
        auth: false,
      },
    },
  ],
});

router.beforeEach((to, from, next) => {
  if (to.matched.some((res) => res.meta.auth)) {
    if (localStorage.token) {
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
