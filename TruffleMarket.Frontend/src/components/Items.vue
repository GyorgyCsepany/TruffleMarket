<script setup>
import { ref } from "vue";
import "vue-good-table-next/dist/vue-good-table-next.css";
import { VueGoodTable } from "vue-good-table-next";

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
    label: "Date of picking",
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

const getItems = async (newParams) => {
  gridRequest = { ...gridRequest, ...newParams };
  const response = await fetch("https://trufflemarketapi.azurewebsites.net/items", {
    method: "POST",
    headers: {
      "Content-Type": "application/json;charset=utf-8",
    },
    body: JSON.stringify(gridRequest),
  });
  const json = await response.json();
  gridRows.value = json.rows;
  rowCount.value = json.totalRows;
};

let gridRows = ref(null);
let rowCount = ref(0);
let gridRequest = {
  page: 1,
  perPage: 10,
};

(async () => await getItems())();
</script>

<template>
  <vue-good-table
    v-if="gridRows"
    mode="remote"
    theme="nocturnal"
    :line-numbers="true"
    :rows="gridRows"
    :columns="gridColumns"
    :totalRows="rowCount"
    :pagination-options="{
      enabled: true,
    }"
    @column-filter="getItems({ ...$event, page: 1 })"
    @page-change="getItems({ page: $event.currentPage })"
    @per-page-change="getItems({ perPage: $event.currentPerPage, page: 1 })"
    @sort-change="getItems({ sort: $event, page: 1 })"
  />
</template>

<style scoped></style>
