<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";

const activeTab = ref("itemsTab");
const router = useRouter();

const handleClick = (tabName) => {
  switch (tabName) {
    case "itemsTab":
      router.push({ name: "Items" });
      break;
    case "loginTab":
      router.push({ name: "LoginOrRegister" });
      break;
    // default:
    //   alert("TODO");
  }
};
</script>

<template>
  <el-container>
    <el-header>
      <el-tabs v-model="activeTab" @tab-click="handleClick($event.props.name)">
        <el-tab-pane label="Active items" name="itemsTab" />
        <el-tab-pane label="Your bids" name="bidsTab" />
        <el-tab-pane label="Your offers" name="offersTab" />
        <el-tab-pane label="Submit an offer" name="submitTab" />
        <el-tab-pane label="Login" name="loginTab" />
      </el-tabs>
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

.el-header {
  text-transform: uppercase;
  background-color: #2c394f;
  line-height: 60px;
  padding-left: max(15px, 10%);
  padding-right: max(15px, 10%);
}

.el-main {
  padding: 20px max(15px, 10%);
}

.el-tabs__header .el-icon svg {
  height: 60px;
}

.el-tabs__item {
  color: rgb(199, 198, 198);
  font-weight: bold;
}

.el-tabs__item.is-active,
.el-tabs__item:hover {
  color: white;
}

.el-tabs__active-bar {
  background-color: white;
}

.el-tabs__nav-wrap::after {
  background-color: transparent;
}
</style>
