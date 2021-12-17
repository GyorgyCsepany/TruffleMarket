<script setup>
import { Coin } from "@element-plus/icons-vue";
import { ref } from "vue";

const formRef = ref({});
const offer = ref({
  weight: 1,
  price: 1,
  certificated: false,
});

const submitForm = () => {
  console.log(offer.value);
  formRef.value.validate((valid) => {
    console.log(valid);
  });
};

const resetForm = () => formRef.value.resetFields();

const checkExpiration = (rule, expDate, callback) => {
  if (!expDate || Date.now() > expDate) {
    callback(new Error("Please give a date in the future!"));
  } else {
    callback();
  }
};

const checkPickingDate = (rule, pickDate, callback) => {
  if (!pickDate || Date.now() < pickDate) {
    callback(new Error("Please give a date in the past!"));
  } else {
    callback();
  }
};

const rules = ref({
  truffle: [
    {
      required: true,
      message: "Please give a type of truffle!",
      trigger: "blur",
    },
  ],
  origin: [
    {
      required: true,
      message: "Please give a origin country!",
      trigger: "blur",
    },
  ],
  expiration: [{ validator: checkExpiration, trigger: "blur" }],
  pickingDate: [{ validator: checkPickingDate, trigger: "blur" }],
});
</script>

<template>
  <div class="NewOffer-container">
    <el-card class="box-card">
      <div class="NewOffer-header">
        <el-icon><coin /></el-icon>
        <h1>New offer</h1>
      </div>
      <el-form ref="formRef" :model="offer" :rules="rules" label-position="top">
        <div class="NewOffer-cardRow">
          <el-form-item label="Truffle" prop="truffle">
            <el-select
              v-model="offer.truffle"
              placeholder="Select a type of truffle"
            >
              <el-option label="Black" value="Black" />
              <el-option label="White" value="White" />
              <el-option label="Summer" value="Summer" />
              <el-option label="Burgundy" value="Burgundy" />
            </el-select>
          </el-form-item>
          <el-form-item label="Weight" prop="weight">
            <el-input-number
              v-model="offer.weight"
              :min="1"
              controls-position="right"
            />
          </el-form-item>
        </div>
        <div class="NewOffer-cardRow">
          <el-form-item label="Origin" prop="origin">
            <el-select
              v-model="offer.origin"
              placeholder="Select an origin country"
            >
              <el-option label="France" value="France" />
              <el-option label="Hungary" value="Hungary" />
              <el-option label="Italy" value="Italy" />
              <el-option label="Spain" value="Spain" />
              <el-option label="United  Kingdom" value="United  Kingdom" />
            </el-select>
          </el-form-item>
          <el-form-item label="Price" prop="price">
            <el-input-number
              v-model="offer.price"
              :precision="2"
              :step="0.01"
              :min="1"
              controls-position="right"
            />
          </el-form-item>
        </div>
        <div class="NewOffer-cardRow">
          <el-form-item label="Picking" prop="pickingDate">
            <el-date-picker
              v-model="offer.pickingDate"
              type="date"
              placeholder="Pick a day"
            />
          </el-form-item>
          <el-form-item label="Expiration" prop="expiration">
            <el-date-picker
              v-model="offer.expiration"
              type="datetime"
              placeholder="Select date and time"
            />
          </el-form-item>
        </div>
        <el-form-item label="Certificated" prop="certificated">
          <el-switch
            v-model="offer.certificated"
            active-color="#2c394f"
            inactive-color="grey"
          />
        </el-form-item>
        <div class="NewOffer-buttonGroup">
          <el-form-item class="button1">
            <el-button color="#2c394f" type="primary" @click="submitForm"
              >Submit</el-button
            >
          </el-form-item>
          <el-form-item class="button2">
            <el-button color="#2c394f" type="primary" @click="resetForm"
              >Reset</el-button
            >
          </el-form-item>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<style>
.NewOffer-container {
  display: flex;
  justify-content: center;
  align-self: center;
}

.NewOffer-header {
  display: flex;
  align-items: center;
}

.NewOffer-cardRow {
  display: flex;
  gap: 75px;
}

.NewOffer-buttonGroup {
  display: flex;
  justify-content: center;
  gap: 75px;
}

.NewOffer-container .el-card {
  background-color: rgb(231, 227, 227);
  width: 35em;
  color: #2c394f !important;
}

.NewOffer-header .el-icon {
  font-size: 40px;
  padding-right: 15px;
}

.NewOffer-container .el-input__prefix-inner {
  align-items: center;
}

.NewOffer-container .el-form-item__label {
  font-weight: bold;
  color: #2c394f;
  font-size: 16px;
  line-height: 10px;
}
</style>
