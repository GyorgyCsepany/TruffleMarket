<script setup>
import { ref } from "vue";
import dayjs from "dayjs";
import {
  SoldOut,
  SuccessFilled,
  WarningFilled,
  CircleCloseFilled,
} from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
const token = localStorage.token;
const tableData = ref();

const showSuccesClose = () => {
  ElMessage({
    message: "Your offer was successfully closed!",
    type: "success",
  });
};

const showErrorClose = () => {
  ElMessage({
    message: `Your offer can not be closed. Please try again later!`,
    type: "error",
  });
};

const getOffers = async () => {
  const offersResponse = await fetch(`https://trufflemarketapi.azurewebsites.net/user/offers`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json;charset=utf-8",
    },
  });
  tableData.value = await offersResponse.json();
};

(async () => await getOffers())();

const onClickClose = async (index, row) => {
  const closeResponse = await fetch(
    `https://trufflemarketapi.azurewebsites.net/items/offer/close`,
    {
      method: "PUT",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json;charset=utf-8",
      },
      body: JSON.stringify({
        itemId: row.itemId,
      }),
    }
  );

  if (closeResponse.ok) {
    showSuccesClose();
  } else {
    showErrorClose();
  }

  await getOffers();
};
</script>

<template>
  <div class="UserOffers-container">
    <el-card class="box-card">
      <div class="UserOffers-header">
        <el-icon><sold-out /></el-icon>
        <h1>Available offers</h1>
      </div>
      <el-table :data="tableData">
        <el-table-column prop="itemId" label="Id" width="50" />
        <el-table-column prop="truffle" label="Truffle" width="100" />
        <el-table-column prop="weight" label="Weight" width="80" />
        <el-table-column prop="price" label="Price" width="60" />
        <el-table-column label="Status" width="120">
          <template #default="scope">
            <div v-if="scope.row.status == 0">
              <el-icon color="green"><success-filled /></el-icon>
              Executed
            </div>
            <div v-else-if="scope.row.status == 1">
              <el-icon color="#ca4e63"><warning-filled /></el-icon>
              Open
            </div>
            <div v-else>
              <el-icon><circle-close-filled color="red" /></el-icon>
              Failed
            </div>
          </template>
        </el-table-column>
        <el-table-column label="Buyer" width="120">
          <template #default="scope">
            <el-tooltip
              v-if="scope.row.buyerName"
              class="item"
              effect="dark"
              placement="top-end"
            >
              <template #content>
                {{ scope.row.buyerEmail }}
              </template>
              <template #default>
                <el-tag size="medium">{{
                  scope.row.buyerName ? scope.row.buyerName : "No buyer yet"
                }}</el-tag>
              </template>
            </el-tooltip>
            <el-tag v-else size="medium">No buyer yet</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="Expiration">
          <template #default="scope">{{
            dayjs(scope.row.expiration).format("YYYY-MM-DD")
          }}</template>
        </el-table-column>
        <el-table-column label="Action" width="80">
          <template #default="scope">
            <el-popconfirm
              confirm-button-text="Yes"
              cancel-button-text="No"
              title="Are you sure to close this?"
              @confirm="onClickClose(scope.$index, scope.row)"
            >
              <template #reference>
                <el-button size="mini" color="#2c394f">Close</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<style>
.UserOffers-container {
  display: flex;
  justify-content: center;
  align-self: center;
}

.UserOffers-header {
  display: flex;
  align-items: center;
}

.UserOffers-container .el-card {
  background-color: rgb(231, 227, 227);
  width: 47em;
  color: #2c394f !important;
}

.UserOffers-header .el-icon {
  font-size: 40px;
  padding-right: 15px;
}

.UserOffers-container .el-tag,
.UserOffers-container .el-button {
  color: white;
  font-weight: bold;
}

.UserOffers-container .el-tag {
  background-color: #2c394f;
}

.UserOffers-container .el-table thead {
  color: white;
}

.UserOffers-container .el-table .cell {
  font-weight: bold;
}

.UserOffers-container .el-table .el-icon {
  font-size: 15px;
  padding-right: 5px;
}

.UserOffers-container .el-table {
  --el-table-text-color: #2c394f;
  --el-table-border-color: black;
  --el-table-border: 1.5px solid var(--el-table-border-color);
  --el-table-header-bg-color: #2e3d57;
  --el-table-tr-bg-color: rgb(199, 193, 193);
}
</style>
