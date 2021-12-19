<script setup>
import { ref } from "vue";
import dayjs from "dayjs";
import { VueGoodTable } from "vue-good-table-next";
import {
  GoodsFilled,
  CircleCheckFilled,
  CircleCloseFilled,
} from "@element-plus/icons-vue";
import "vue-good-table-next/dist/vue-good-table-next.css";

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
    label: "Picking date",
    field: "pickingDate",
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
    field: "sellerName",
  },
];

const getItems = async (newParams) => {
  gridRequest = { ...gridRequest, ...newParams };
  const itemsResponse = await fetch(
    "https://trufflemarketapi.azurewebsites.net/items",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json;charset=utf-8",
      },
      body: JSON.stringify(gridRequest),
    }
  );
  const itemsJson = await itemsResponse.json();
  gridRows.value = itemsJson.rows;
  rowCount.value = itemsJson.totalRows;
};

const loggedInUser = ref(localStorage.user && JSON.parse(localStorage.user));
const gridRows = ref(null);
const rowCount = ref(0);
let gridRequest = {
  page: 1,
  perPage: 10,
};

const biddingPrice = ref(0);

const bidDialogVisible = ref(false);

const clickedItem = ref({});

(async () => await getItems())();

const onRowClick = async (item) => {
  bidDialogVisible.value = true;
  biddingPrice.value = item.row.price + 0.01;
  clickedItem.value = item.row;

  const itemResponse = await fetch(
    `https://trufflemarketapi.azurewebsites.net/item/${item.row.itemId}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json;charset=utf-8",
      },
    }
  );

  const itemJson = await itemResponse.json();

  clickedItem.value = {
    ...clickedItem.value,
    ...itemJson,
    pickingDate: dayjs(clickedItem.value.pickingDate).format("YYYY-MM-DD"),
    expiration: dayjs(clickedItem.value.expiration).format(
      "YYYY-MM-DD HH:mm:ss"
    ),
  };
};

const makeABid = async () => {
  const bidResponse = await fetch(
    `https://trufflemarketapi.azurewebsites.net/item/${clickedItem.value.itemId}/bid`,
    {
      method: "PUT",
      headers: {
        Authorization: `Bearer ${loggedInUser.value.token}`,
        "Content-Type": "application/json;charset=utf-8",
      },
      body: JSON.stringify({
        bidPrice: biddingPrice.value,
        buyerId: loggedInUser.value.userId,
      }),
    }
  );

  if (bidResponse.ok) {
    bidDialogVisible.value = false;
  } else {
    console.log("Redirect to YOURBIDS!");
  }
};
</script>

<template>
  <div class="Items-container">
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
      @row-click="onRowClick"
      @column-filter="getItems({ ...$event, page: 1 })"
      @page-change="getItems({ page: $event.currentPage })"
      @per-page-change="getItems({ perPage: $event.currentPerPage, page: 1 })"
      @sort-change="getItems({ sort: $event, page: 1 })"
    />
    <el-dialog v-if="clickedItem" v-model="bidDialogVisible">
      <template #title>
        <el-icon class="Items-headerIcon"><goods-filled /></el-icon>
        <h1>{{ clickedItem.truffle + " " + clickedItem.itemId }}</h1>
      </template>
      <el-descriptions class="margin-top" :column="4" direction="vertical">
        <el-descriptions-item label="Current price">{{
          clickedItem.price
        }}</el-descriptions-item>
        <el-descriptions-item label="Weight">{{
          clickedItem.weight
        }}</el-descriptions-item>
        <el-descriptions-item label="Origin">{{
          clickedItem.origin
        }}</el-descriptions-item>
        <el-descriptions-item label="Expiration">{{
          clickedItem.expiration
        }}</el-descriptions-item>
        <el-descriptions-item label="Certificated">
          <el-icon v-if="clickedItem.certificated" color="green"
            ><circle-check-filled
          /></el-icon>
          <el-icon v-else color="red"><circle-close-filled /></el-icon>
        </el-descriptions-item>
        <el-descriptions-item label="Seller">{{
          clickedItem.sellerName
        }}</el-descriptions-item>
        <el-descriptions-item label="Seller rate">
          <el-rate v-model="clickedItem.sellerRate" disabled> </el-rate>
        </el-descriptions-item>
        <el-descriptions-item label="Picking date">
          {{ clickedItem.pickingDate }}
        </el-descriptions-item>
        <el-descriptions-item label="Description">
          {{ clickedItem.description }}
        </el-descriptions-item>
      </el-descriptions>
      <template v-if="loggedInUser" #footer>
        <el-input-number
          v-model="biddingPrice"
          :precision="2"
          :step="0.01"
          :min="clickedItem.price + 0.01"
          controls-position="left"
          size="medium"
        />
        <el-button color="#2c394f" width="20px" type="primary" @click="makeABid"
          >Make a bid</el-button
        ></template
      >
      <template v-else #footer> To make a bid, please log in first!</template>
    </el-dialog>
  </div>
</template>

<style>
.Items-container .vgt-table tr:hover td {
  background-color: rgb(39, 41, 43);
  cursor: pointer;
}

.Items-container .el-dialog {
  background-color: rgb(231, 227, 227);
  color: #2c394f !important;
}

.Items-container .el-dialog__header {
  display: flex;
  align-items: center;
}

.Items-container .el-dialog__footer {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 50px;
}

.Items-container .el-dialog__body {
  padding-top: 0px;
  padding-bottom: 0px;
}

.Items-container .Items-headerIcon {
  font-size: 40px;
  padding-right: 15px;
}

.Items-container .el-descriptions__body {
  background-color: rgb(231, 227, 227);
}

.Items-container .el-descriptions__label {
  font-size: 16px;
  font-weight: bold !important;
  padding-bottom: 4px !important;
  color: #2c394f;
}
</style>
