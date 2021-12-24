<script setup>
import { ref } from "vue";
import { ElMessage } from "element-plus";
import { useRouter } from "vue-router";
import "element-plus/es/components/message/style/css";

const router = useRouter();
const userInput = ref({});
const formRef = ref({});
const isRegistration = ref(true);
const user = ref(localStorage.user);

const showError = () => {
  ElMessage({
    message: "The user can not be authenticated! Please try again!",
    type: "error",
  });
};

const showSuccess = () => {
  ElMessage({
    message: "Your are successfully authenticated!",
    type: "success",
  });
};

const logout = () => {
  user.value = null;
  localStorage.clear();
};

const submitForm = () => {
  formRef.value.validate(async (valid) => {
    if (valid) {
      const response = await fetch(
        "https://trufflemarketapi.azurewebsites.net/user",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json;charset=utf-8",
          },
          body: JSON.stringify({
            ...userInput.value,
            isLogin: isRegistration.value ? false : true,
          }),
        }
      );

      if (response.ok) {
        showSuccess();
        const responseJson =  await response.json();
        localStorage.token = responseJson.token;
        localStorage.user = responseJson.name;
        document.getElementsByClassName("el-menu-item")[0].click();
        router.push({ name: "Items" });
      } else {
        formRef.value.resetFields();
        showError();
      }
    }
  });
};

const selectRegistration = (isRegSelected) => {
  formRef.value.resetFields();
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

const checkEmail = (rule, email, callback) => {
  var regExp = /\S+@\S+\.\S+/;

  if (regExp.test(email)) {
    callback();
  } else {
    callback(new Error("Please give a valid email!"));
  }
};

const rules = ref({
  name: [{ required: true, message: "Please input name!", trigger: "blur" }],
  password: [
    { required: true, message: "Please give a password!", trigger: "blur" },
  ],
  confirmPassword: [{ validator: checkConfirmPassword, trigger: "blur" }],
  email: [{ validator: checkEmail, trigger: "blur" }],
});
</script>

<template>
  <div class="UserPage-container">
    <el-card v-if="user" class="box-card">
      <h2>Welcome {{user}}!</h2>
      <el-button color="#2c394f" type="primary" @click="logout"
        >Logout</el-button
      >
    </el-card>
    <el-card v-else class="box-card">
      <h2>{{ isRegistration ? "Register" : "Login" }}</h2>
      <el-form
        ref="formRef"
        :model="userInput"
        :rules="rules"
        label-position="top"
      >
        <el-form-item label="Name" prop="user">
          <el-input v-model="userInput.name" />
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
        <el-form-item v-if="isRegistration" label="Email" prop="email">
          <el-input v-model="userInput.email" />
        </el-form-item>
        <el-form-item>
          <el-button color="#2c394f" type="primary" @click="submitForm">
            {{ isRegistration ? "Register" : "Login" }}
          </el-button>
        </el-form-item>
      </el-form>
      <div v-if="isRegistration" class="UserPage-footer">
        Already have an account?
        <el-link :underline="false" @click="selectRegistration(false)"
          >Login</el-link
        >
      </div>
      <div v-else class="UserPage-footer">
        Don't have an account?
        <el-link :underline="false" @click="selectRegistration(true)"
          >Register</el-link
        >
      </div>
    </el-card>
  </div>
</template>

<style>
.UserPage-container {
  display: flex;
  justify-content: center;
}

.UserPage-footer {
  font-size: 14px;
  text-align: center;
}

.UserPage-footer .el-link {
  font-weight: bold;
  color: #2c394f;
}

.UserPage-container .el-card {
  background-color: rgb(231, 227, 227);
  width: 25rem;
  color: #2c394f !important;
}

.UserPage-container .el-form-item__label {
  line-height: 15px;
  font-weight: bold;
  color: #2c394f;
}
</style>
