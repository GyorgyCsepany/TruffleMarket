<script setup>
import { useRouter } from "vue-router";
import { UserFilled } from "@element-plus/icons-vue";

const router = useRouter();

const handleMenuItemSelect = (menuIndex) => {
  const user = localStorage.user && JSON.parse(localStorage.user);
  const userId = user ? user.userId : undefined;
  switch (menuIndex) {
    case "1":
      router.push({ name: "Items" });
      break;
    case "2":
      router.push({ name: "UserBids", params: { userId: userId } });
      break;
    case "3":
      router.push({ name: "UserOffers", params: { userId: userId } });
      break;
    case "4":
      router.push({
        name: "NewOffer",
        params: { userId: userId },
      });
      break;
    case "5":
      router.push({ name: "LoginOrRegister" });
      break;
  }
};
</script>

<template>
  <el-container>
    <el-header>
      <el-menu
        ref="formRef"
        default-active="1"
        class="el-menu-demo"
        mode="horizontal"
        background-color="#2c394f"
        text-color="rgb(199, 198, 198)"
        active-text-color="white"
        @select="handleMenuItemSelect"
      >
        <el-menu-item index="1">Active items</el-menu-item>
        <el-menu-item index="2">Your bids</el-menu-item>
        <el-menu-item index="3">Your offers</el-menu-item>
        <el-menu-item index="4">Submit and offer</el-menu-item>
        <el-menu-item index="5"
          ><el-icon><user-filled /></el-icon
        ></el-menu-item>
      </el-menu>
    </el-header>
    <el-main><router-view /></el-main>
  </el-container>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
}

body {
  margin: 0px;
}

.el-menu-item {
  background-color: #2c394f !important;
}

.el-menu-item.is-active,
.el-menu-item:hover {
  color: white !important;
  background-color: #2c394f !important;
}

.el-menu-item .el-icon {
  font-size: 20px;
}

.el-header {
  text-transform: uppercase;
  background-color: #2c394f;
  padding-left: max(15px, 10%);
  padding-right: max(15px, 10%);
  font-weight: bold;
}

.el-main {
  padding: 20px max(15px, 10%);
}
</style>
