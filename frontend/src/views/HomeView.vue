<template>
  <div>
    <h1>Home</h1>
    <button type="button" class="btn btn-primary" @click="navigateToManageTag">
      Manage Tags
    </button>
  </div>
  <div>
    <h1>Tag IDs</h1>
    <ul>
      <li v-for="tagId in tagIds" :key="tagId">{{ tagId }}</li>
    </ul>
  </div>
</template>

<script lang="ts">
import axios from 'axios';
export default {
  name: "HomeView",
  methods: {
    navigateToManageTag() {
      this.$router.push("/ManageTag");
    },
     async fetchTagIds() {
      try {
        // actual backend API endpoint
        const response = await axios.get('http://localhost:3000/getScanResult');
        this.tagIds = response.data; //  an array of tag IDs
      } catch (error) {
        console.error('Error fetching tag IDs:', error);
      }
    },
  },

  data() {
    return {
      tagIds: [],
    };
  },

  mounted() {
    // Fetch tag IDs every second
    this.fetchTagIds();
    setInterval(this.fetchTagIds, 1000);
  },
  
};
</script>
