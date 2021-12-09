<script setup>
import { ref } from "vue";
import "vue-good-table-next/dist/vue-good-table-next.css";
import { VueGoodTable } from "vue-good-table-next";

let gridRows = ref(null);

const gridColumns = [
  {
    label: "Truffle",
    field: "truffle",
    filterOptions: {
      enabled: true,
      placeholder: "Filter type",
      filterDropdownItems: ["Black", "White", "Burgundy", "Garlic"],
    },
  },
  {
    label: "Weight",
    field: "weight",
    type: "number",
  },
  {
    label: "Price",
    field: "price",
    type: "decimal",
  },
  {
    label: "Origin",
    field: "origin",
  },
  {
    label: "Date Of picking",
    field: "dateOfPicking",
    type: "date",
    dateInputFormat: "yyyy-MM-dd'T'HH:mm:ssXXX",
    dateOutputFormat: "yyyy-MM-dd",
  },
  {
    label: "Certificated",
    field: "certificated",
    type: "boolean",
  },
  {
    label: "Expiration",
    field: "expiration",
    type: "date",
    dateInputFormat: "yyyy-MM-dd'T'HH:mm:ssXXX",
    dateOutputFormat: "yyyy-MM-dd",
  },
  {
    label: "Seller",
    field: "seller",
  },
];

(async () => {
  const response = await fetch(
    "https://trufflemarketapi.azurewebsites.net/items"
  );
  const json = await response.json();
  gridRows.value = json;
})();
</script>

<template>
  <vue-good-table
    v-if="gridRows"
    :line-numbers="true"
    :columns="gridColumns"
    :rows="gridRows"
    :pagination-options="{
      enabled: true,
      perPage: 10,
    }"
    theme="nocturnal"
  />
</template>

<style scoped></style>
