<script setup>
import { ref } from "vue";
import { ElMessage } from "element-plus";
import { useRouter } from "vue-router";
import "element-plus/es/components/message/style/css";

const router = useRouter();
const userInput = ref({});
const formRef = ref({});
const isRegistration = ref(true);

const showError = () => {
  ElMessage({
    message: "The user can not be authenticated! Please try again!",
    type: "error",
  });
};

const submitForm = () => {
  formRef.value.validate(async (valid) => {
    if (valid) {
      const response = await fetch(
        `https://trufflemarketapi.azurewebsites.net/${
          isRegistration.value ? "register" : "login"
        }`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json;charset=utf-8",
          },
          body: JSON.stringify({
            name: userInput.value.user,
            password: userInput.value.password,
          }),
        }
      );

      if (response.ok) {
        localStorage.user = await response.text();
        router.push({ name: "Items" });
      } else {
        formRef.value.resetFields();
        showError();
      }
    }
  });
};

const selectRegistration = (isRegSelected) => {
  isRegistration.value = isRegSelected;
};

const checkConfirmPassword = (rule, confirmPassword, callback) => {
  if (confirmPassword === "") {
    callback(new Error("Please input the password again"));
  } else if (
    isRegistration.value &&
    confirmPassword !== userInput.value.password
  ) {
    callback(new Error("Two passwords don't match!"));
  } else {
    callback();
  }
};

const rules = ref({
  user: [{ required: true, message: "Please input name", trigger: "blur" }],
  password: [
    { required: true, message: "Please give a password", trigger: "blur" },
  ],
  confirmPassword: [{ validator: checkConfirmPassword, trigger: "blur" }],
});
</script>

<template>
  <div class="Login-container">
    <el-card class="box-card">
      <h2 v-if="isRegistration">Register</h2>
      <h2 v-else>Login</h2>
      <el-form
        ref="formRef"
        :model="userInput"
        :rules="rules"
        label-position="top"
      >
        <el-form-item label="Name" prop="user">
          <el-input v-model="userInput.user" />
        </el-form-item>
        <el-form-item label="Password" prop="password">
          <el-input v-model="userInput.password" />
        </el-form-item>
        <el-form-item
          v-if="isRegistration"
          label="Confirm password"
          prop="confirmPassword"
        >
          <el-input v-model="userInput.confirmPassword" />
        </el-form-item>
        <el-form-item v-if="isRegistration">
          <el-button color="#2c394f" type="primary" @click="submitForm"
            >Register</el-button
          >
        </el-form-item>
        <el-form-item v-else>
          <el-button color="#2c394f" type="primary" @click="submitForm"
            >Login</el-button
          >
        </el-form-item>
      </el-form>
      <div v-if="isRegistration" class="Login-footer">
        Already have an account?
        <el-link :underline="false" @click="selectRegistration(false)"
          >Login</el-link
        >
      </div>
      <div v-else class="Login-footer">
        Don't have an account?
        <el-link :underline="false" @click="selectRegistration(true)"
          >Register</el-link
        >
      </div>
    </el-card>
  </div>
</template>

<style>
.Login-container {
  display: flex;
  justify-content: center;
}

.Login-footer {
  font-size: 14px;
  text-align: center;
}

.Login-footer .el-link {
  font-weight: bold;
  color: #2c394f;
}

.Login-container .el-card {
  background-color: rgb(231, 227, 227);
  width: 25rem;
  color: #2c394f !important;
}

.Login-container .el-form-item__label {
  line-height: 15px;
  font-weight: bold;
  color: #2c394f;
}
</style>
