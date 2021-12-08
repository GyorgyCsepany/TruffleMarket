<script setup>
import { ref } from "vue";

let randomItem = ref(null);

const getItems = async () => {
  const response = await fetch(
    "https://trufflemarketapi.azurewebsites.net/randomitem"
  );
  const json = await response.json();
  randomItem.value = json;
};
</script>

<template>
  <h1>{{ msg }}</h1>
  <el-button type="primary" plain @click="getItems">Get Item</el-button>
  <p v-if="randomItem">{{ randomItem }}</p>
</template>

<style scoped></style>
