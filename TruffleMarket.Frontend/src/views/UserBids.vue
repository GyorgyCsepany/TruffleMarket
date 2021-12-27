<script setup>
import { ref } from "vue";
import dayjs from "dayjs";
import {
  TrendCharts,
  SuccessFilled,
  WarningFilled,
} from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";

const truffleApiUrl = import.meta.env.VITE_TRUFFLE_API_URL;

const token = localStorage.token;
const tableData = ref();
const isCloseDialogOpen = ref(false);
const closeRow = ref({});

const showSuccesClose = () => {
  ElMessage({
    message: "Your bid was successfully closed!",
    type: "success",
  });
};

const showErrorClose = () => {
  ElMessage({
    message: `Your bid can not be closed. Please try again later!`,
    type: "error",
  });
};

const getBids = async () => {
  const bidsResponse = await fetch(`${truffleApiUrl}/user/bids`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json;charset=utf-8",
    },
  });
  tableData.value = await bidsResponse.json();
};

(async () => await getBids())();

const onClickClose = (index, row) => {
  isCloseDialogOpen.value = true;
  closeRow.value.itemId = row.itemId;
  closeRow.value.sellerRate = null;
  closeRow.value.sellerId = row.sellerId;
  closeRow.value.name = row.sellerName;
};

const closeRowItem = async () => {
  const closeResponse = await fetch(`${truffleApiUrl}/user/bid/close`, {
    method: "PUT",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json;charset=utf-8",
    },
    body: JSON.stringify(closeRow.value),
  });

  if (closeResponse.ok) {
    showSuccesClose();
  } else {
    showErrorClose();
  }

  await getBids();
  isCloseDialogOpen.value = false;
};
</script>

<template>
  <div class="NewBids-container">
    <el-card class="box-card">
      <div class="NewBids-header">
        <el-icon><trend-charts /></el-icon>
        <h1>Available bids</h1>
      </div>
      <el-table :data="tableData">
        <el-table-column prop="itemId" label="Id" width="50" />
        <el-table-column prop="truffle" label="Truffle" width="100" />
        <el-table-column prop="weight" label="Weight" width="80" />
        <el-table-column prop="price" label="Price" width="60" />
        <el-table-column label="Status" width="120">
          <template #default="scope"
            ><el-icon v-if="scope.row.isExecuted" color="green"
              ><success-filled
            /></el-icon>
            <el-icon v-else color="#ca4e63"><warning-filled /></el-icon>
            {{ scope.row.isExecuted ? "Executed" : "Open" }}</template
          >
        </el-table-column>
        <el-table-column label="Seller" width="100">
          <template #default="scope">
            <el-tooltip class="item" effect="dark" placement="top-end">
              <template #content>
                {{ scope.row.sellerEmail }}
              </template>
              <template #default>
                <el-tag size="medium">{{ scope.row.sellerName }}</el-tag>
              </template>
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column label="Expiration" width="100">
          <template #default="scope">{{
            dayjs(scope.row.expiration).format("YYYY-MM-DD")
          }}</template>
        </el-table-column>
        <el-table-column label="Action" width="85">
          <template #default="scope">
            <el-button
              size="mini"
              color="#2c394f"
              :disabled="!scope.row.isExecuted"
              @click="onClickClose(scope.$index, scope.row)"
              >Close</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-card>
    <el-dialog v-model="isCloseDialogOpen" width="30%" center>
      <p>
        If you are sure to close this item from your side, you can rate here its
        seller.
      </p>
      {{ closeRow.name }}
      <el-rate v-model="closeRow.sellerRate" allow-half>
        {{ closeRow.name }}</el-rate
      >
      <template #footer>
        <span class="dialog-footer">
          <el-button type="primary" color="#2c394f" @click="closeRowItem"
            >Confirm</el-button
          >
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<style>
.NewBids-container {
  display: flex;
  justify-content: center;
  align-self: center;
}

.NewBids-header {
  display: flex;
  align-items: center;
}

.NewBids-container .el-dialog {
  background-color: rgb(231, 227, 227);
  width: 25%;
}

.NewBids-container .el-dialog__body {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-size: 14px;
  font-weight: bold;
  word-break: keep-all;
}

.NewBids-container .el-card {
  background-color: rgb(231, 227, 227);
  width: 46em;
  color: #2c394f !important;
}

.NewBids-header .el-icon {
  font-size: 40px;
  padding-right: 15px;
}

.NewBids-container .el-tag,
.NewBids-container .el-button {
  color: white;
  font-weight: bold;
}

.NewBids-container .el-tag {
  background-color: #2c394f;
}

.NewBids-container .el-table thead {
  color: white;
}

.NewBids-container .el-table .cell {
  font-weight: bold;
}

.NewBids-container .el-table .el-icon {
  font-size: 15px;
  padding-right: 5px;
}

.NewBids-container .el-table {
  --el-table-text-color: #2c394f;
  --el-table-border-color: black;
  --el-table-border: 1.5px solid var(--el-table-border-color);
  --el-table-header-bg-color: #2e3d57;
  --el-table-tr-bg-color: rgb(199, 193, 193);
}
</style>
