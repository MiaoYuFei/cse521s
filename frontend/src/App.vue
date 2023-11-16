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
