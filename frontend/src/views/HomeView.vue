<template>
  <div>
    <BsNavbar />
    <div class="container">
      <div class="card m-2">
        <div class="card-body">
          <h1 class="card-title">
            Tag IDs
          </h1>
          <div class="card-text">
            <h5 v-if="tagIds.length <= 0">
              No tag detected!
            </h5>
            <div
              v-if="tagIds.length > 0"
              class="d-flex"
              style="justify-content: space-between;"
            >
              <div class="border w-100 p-1 fs-1">
                <span>Required Items</span>
                <ul class="list-unstyled">
                  <li
                    v-for="tagId in tagIds"
                    :key="tagId"
                    style="background-color: rgb(0, 200, 0);"
                    class="mx-2 my-2 p-2"
                  >
                    <span style="font-weight: bold;">Item Name</span>
                    <div class="fs-3">
                      <span>Tag Id:</span>
                      <span>{{ tagId }}</span>
                    </div>
                  </li>
                </ul>
              </div>
              <div class="border w-100 p-1 fs-1">
                <span>Distractor Items</span>
                <ul class="list-unstyled">
                  <li
                    v-for="tagId in tagIds"
                    :key="tagId"
                    style="background-color: rgb(255, 65, 65);"
                    class="mx-2 my-2 p-2"
                  >
                    <span style="font-weight: bold;">Item Name</span>
                    <div class="fs-3">
                      <span>Tag Id:</span>
                      <span>{{ tagId }}</span>
                    </div>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BsNavbar from "@/components/BsNavbar.vue";
import axios from "axios";

export default {
    name: "HomeView",
    components: { BsNavbar },
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
    methods: {
        navigateToManageTag() {
            this.$router.push("/ManageTag");
        },
        async fetchTagIds() {
            try {
                // actual backend API endpoint
                const response = await axios.post("/api/getScanResult");
                this.tagIds = response.data.tags; //  an array of tag IDs
            }
            catch (error) {
                console.error("Error fetching tag IDs:", error);
            }
        },
    }
};
</script>
