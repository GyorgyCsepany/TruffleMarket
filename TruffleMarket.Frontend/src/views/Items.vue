<script setup>
import { ref } from "vue";
import dayjs from "dayjs";
import { VueGoodTable } from "vue-good-table-next";
import {
  GoodsFilled,
  CircleCheckFilled,
  CircleCloseFilled,
  Briefcase,
} from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import "element-plus/es/components/message/style/css";
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
];

const getItems = async (newParams) => {
  gridRequest = { ...gridRequest, ...newParams };

  const filterTruffle = gridRequest.columnFilters
    ? gridRequest.columnFilters.truffle
    : "";
  const sortField = gridRequest.sort ? gridRequest.sort[0].field : "";
  const sortType = gridRequest.sort ? gridRequest.sort[0].type : "";
  const requestUrl = `https:/trufflemarketapi.azurewebsites.net/items?page=${gridRequest.page}&perPage=${gridRequest.perPage}&filterTruffle=${filterTruffle}&sortField=${sortField}&sortType=${sortType}`;

  const itemsResponse = await fetch(requestUrl, {
    method: "GET",
  });

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

const batchBidFormRef = ref({});
const batchBidDialogVisible = ref(false);
const batchBidModel = ref({});
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

const onBatchBidButtonClick = () => {
  batchBidDialogVisible.value = true;
  batchBidModel.value.buyerId = loggedInUser.value.userId;
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
        itemId: clickedItem.value.itemId,
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

const submitBatchBid = async () => {
  batchBidFormRef.value.validate(async (valid) => {
    if (valid) {
      const batchBidResponse = await fetch(
        `https://trufflemarketapi.azurewebsites.net/items/batch`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${loggedInUser.value.token}`,
            "Content-Type": "application/json;charset=utf-8",
          },
          body: JSON.stringify(batchBidModel.value),
        }
      );
      if (batchBidResponse.ok) {
        const batchBidResponseJson = await batchBidResponse.json();
        showBatchBidSuccess(batchBidResponseJson);
      } else {
        showBatchBidError();
        console.log("Redirect to YOURBIDS!");
      }
      batchBidDialogVisible.value = false;
    }
  });
};

const showBatchBidError = () => {
  ElMessage({
    message: "Your batch bid can not executed. Try again later!",
    type: "error",
  });
};

const showBatchBidSuccess = (batchBidResponseJson) => {
  ElMessage({
    message: `Your bid for ${batchBidResponseJson.includedItemsId.length} items successfully. Total price: ${batchBidResponseJson.totalPrice} Total weight: ${batchBidResponseJson.totalWeight}`,
    type: "success",
  });
};

const resetBatchDialog = () => batchBidFormRef.value.resetFields();
</script>

<template>
  <div class="Items-container">
    <el-button
      class="Items-batchBidButton"
      color="#2c394f"
      width="20px"
      type="primary"
      :disabled="!loggedInUser"
      @click="onBatchBidButtonClick"
      >Batch bid</el-button
    >
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
          size="medium"
        />
        <el-button color="#2c394f" width="20px" type="primary" @click="makeABid"
          >Make a bid</el-button
        ></template
      >
      <template v-else #footer> To make a bid, please log in first!</template>
    </el-dialog>
    <el-dialog v-model="batchBidDialogVisible" @close="resetBatchDialog">
      <template #title>
        <el-icon class="Items-headerIcon"><briefcase /></el-icon>
        <h1>Batch bid</h1>
      </template>
      <el-form ref="batchBidFormRef" :model="batchBidModel">
        <el-form-item
          label="Truffle"
          prop="truffle"
          :rules="[
            {
              required: true,
              message: 'Please select the type of truffle',
              trigger: 'blur',
            },
          ]"
        >
          <el-select
            v-model="batchBidModel.truffle"
            placeholder="Select a type of truffle"
          >
            <el-option label="Black" value="Black" />
            <el-option label="White" value="White" />
            <el-option label="Garlic" value="Garlic" />
            <el-option label="Burgundy" value="Burgundy" />
          </el-select>
        </el-form-item>
        <label>{{
          `Max total price: ${
            batchBidModel.maxTotalPrice ? batchBidModel.maxTotalPrice : ""
          }`
        }}</label>
        <el-form-item
          prop="maxTotalPrice"
          :rules="[
            {
              required: true,
              message: 'Please give a max amount you will bid',
              trigger: 'blur',
            },
          ]"
          ><el-slider
            v-model="batchBidModel.maxTotalPrice"
            :step="0.01"
            :max="1000"
          ></el-slider
        ></el-form-item>
        <label>{{
          `Min total weight: ${
            batchBidModel.minTotalWeight ? batchBidModel.minTotalWeight : ""
          }`
        }}</label>
        <el-form-item
          prop="minTotalWeight"
          :rules="[
            {
              required: true,
              message: 'Please give a min weight you will receive',
              trigger: 'blur',
            },
          ]"
          ><el-slider
            v-model="batchBidModel.minTotalWeight"
            :max="1000"
          ></el-slider
        ></el-form-item>
      </el-form>
      <template #footer>
        <el-button
          color="#2c394f"
          width="20px"
          type="primary"
          @click="submitBatchBid"
          >Submit batch bid</el-button
        >
      </template>
    </el-dialog>
  </div>
</template>

<style>
.Items-batchBidButton {
  margin-bottom: 10px;
  font-size: 16px;
  font-weight: bold;
}

.Items-container .el-slider {
  padding-left: 10px;
}

.Items-container .el-slider__bar {
  background-color: #2c394f;
}

.Items-container .el-slider__button {
  border: solid 2px #2c394f;
}

.Items-container .el-slider__runway {
  background-color: white;
}

.Items-container .el-form-item__label,
.Items-container label {
  font-weight: bold;
  color: #2c394f;
  font-size: 16px;
  padding-right: 40px;
}

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
