module.exports = {
  env: {
    node: true,
  },
  globals: {
    defineProps: "readonly",
  },
  extends: ["eslint:recommended", "plugin:vue/vue3-recommended", "prettier"],
  rules: {
    // override/add rules settings here, such as:
    // 'vue/no-unused-vars': 'error'
  },
};
