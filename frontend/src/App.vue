<script lang="ts">
import { RouterView } from "vue-router";

export default {
  name: "App",
  components: {
    RouterView,
  },
};
</script>

<template>
  <div>
    <RouterView
      class="flex-fill"
      style="min-height: 0"
    />
  </div>
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Nunito&display=swap');
</style>
