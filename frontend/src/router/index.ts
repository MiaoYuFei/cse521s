import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      alias: "/tutorials",
      name: "home",
      component: () => import("/Users/xuruowen/Downloads/cse521s-main/frontend/src/views/HomeView.vue"),
    },
    {
      path: "/ManageTag",
      name: "manageTag",
      component: () => import("/Users/xuruowen/Downloads/cse521s-main/frontend/src/views/ManageTagView.vue"),
    },
  ],
});

export default router;
