<script lang="ts">
import { RouterLink } from "vue-router";

export default {
  components: { RouterLink },
};
</script>
<template>
  <nav
    class="navbar navbar-expand-lg navbar-dark bg-dark"
    aria-label="Navigation"
  >
    <div class="container-fluid">
      <RouterLink
        class="navbar-brand d-flex align-items-center gap-1"
        to="/"
      >
        <img
          src="/src/assets/vue.svg"
          alt="Logo"
          width="30"
          height="24"
          class="d-inline-block align-text-top"
        >
        IoT-based Smart Counter
      </RouterLink>
      <div
        class="navbar-collapse"
      >
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <RouterLink
              class="nav-link"
              to="/"
            >
              <span>Home</span>
            </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink
              class="nav-link"
              to="/ManageTag"
            >
              <span>Manage Tags</span>
            </RouterLink>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
<style scoped>
.dropdown-menu-dark .dropdown-divider {
  border-top: 1px solid rgba(var(--bs-secondary-rgb), 0.675);
}

.nav-link.router-link-active {
  color: #fff;
}
</style>
