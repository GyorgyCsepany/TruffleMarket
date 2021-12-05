module.exports = {
  env: {
    node: true,
  },
  globals: {
    defineProps: "readonly",
  },
  extends: ["eslint:recommended", "plugin:vue/vue3-recommended", "prettier"],
  rules: {
    "vue/require-default-prop": "off",
  },
};
