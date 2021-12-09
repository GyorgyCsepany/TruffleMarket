<script setup>
import Items from "./components/Items.vue";
import { ref } from "vue";

let activeTab = ref("firstTab");
</script>

<template>
  <el-container>
    <el-header>
      <el-tabs v-model="activeTab" @tab-click="handleClick">
        <el-tab-pane label="Active items" name="firstTab"></el-tab-pane>
        <el-tab-pane label="Your bids" name="secondTab"></el-tab-pane>
        <el-tab-pane label="Your offers" name="thirdTab"></el-tab-pane>
        <el-tab-pane label="Submit an offer" name="fourthTab"></el-tab-pane>
      </el-tabs>
    </el-header>
    <el-main><Items /></el-main>
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
  background-color: #2C394F;
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
  font-weight: 600;
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
