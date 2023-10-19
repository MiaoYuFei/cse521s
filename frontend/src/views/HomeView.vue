<template>
  <div>
    <h1>Home</h1>
    <button type="button" class="btn btn-primary" @click="navigateToManageTag">
      Manage Tags
    </button>
  </div>
</template>

<script lang="ts">
export default {
  name: "HomeView",
  methods: {
    navigateToManageTag() {
      this.$router.push("/ManageTag");
    },
  },
};
</script>
