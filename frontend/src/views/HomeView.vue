<template>
  <div>
    <BsNavbar />
    <div
      class="container"
      style="font-family: 'Nunito', sans-serif;"
    >
      <div class="card m-2">
        <div class="card-body">
          <h1 class="card-title">
            <i
              class="bi bi-tag"
              style="color: rgb(94, 94, 94);"
            />Tag Ids
          </h1>
          <div class="card-text">
            <h5 v-if="tags.length <= 0">
              <strong>No tag detected!</strong>
            </h5>
            <div
              v-if="tags.length > 0"
              class="container"
              style="justify-content: space-between;"
            >
              <div class="row mb-3">
                <div class="col-12">
                  <div class="border w-100 p-2 fs-2">
                    <span><i
                      class="bi bi-check-lg"
                      style="color: rgb(25, 196, 25);"
                    />Required Items</span>
                    <h5 v-if="requiredTags.length <= 0">
                      <strong>No required tag detected!</strong>
                    </h5>
                    <ul class="list-unstyled">
                      <li
                        v-for="tag in requiredTags"
                        :key="tag.tag_id"
                        style="background-color: #8B9DC3;"
                        class="m-1 p-2"
                      >
                        <span style="font-weight: bold;">{{ tag.name }}</span>
                        <div class="fs-5">
                          <span>Tag Id: </span>
                          <span>{{ tag.tag_id }}</span>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
              <hr>
              <div class="row mb-3">
                <div class="col-12 col-md-6">
                  <div class="border w-100 p-2 fs-2">
                    <span><i
                      class="bi bi-x-lg"
                      style="color: rgb(250, 65, 65);"
                    />Known Distractor Items</span>
                    <h5 v-if="knownDistractorTags.length <= 0">
                      <strong>No known distractor tag detected!</strong>
                    </h5>
                    <ul class="list-unstyled">
                      <li
                        v-for="tag in knownDistractorTags"
                        :key="tag.tag_id"
                        style="background-color: #DFE3EE;"
                        class="m-1 p-2"
                      >
                        <span style="font-weight: bold;">{{ tag.name }}</span>
                        <div class="fs-5">
                          <span>Tag Id: </span>
                          <span>{{ tag.tag_id }}</span>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
                <div class="col-12 col-md-6">
                  <div class="border w-100 p-2 fs-2">
                    <span><i
                      class="bi bi-question-lg"
                      style="color: rgb(209, 209, 80);"
                    />Unknown Distractor Items</span>
                    <h5 v-if="unknownDistractorTags.length <= 0">
                      <strong>No unknown distractor tag detected!</strong>
                    </h5>
                    <ul class="list-unstyled">
                      <li
                        v-for="tag in unknownDistractorTags"
                        :key="tag.tag_id"
                        style="background-color: #DFE3EE;"
                        class="m-1 p-2"
                      >
                        <div class="fs-5">
                          <span style="font-weight: bold;">Tag Id: </span>
                          <span>{{ tag.tag_id }}</span>
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
    </div>
  </div>
</template>

<script lang="ts">
import BsNavbar from "@/components/BsNavbar.vue";
import TagItem from "@/views/ManageTagView.vue";
import axios from "axios";

export default {
  name: "HomeView",
  components: { BsNavbar },
  data() {
    return {
      tags: [] as TagItem[],
    };
  },
  computed: {
    requiredTags() {
      return this.tags.filter((tag: TagItem) => tag.is_distractor != true);
    },
    knownDistractorTags() {
      return this.tags.filter((tag: TagItem) => tag.is_distractor === true && tag.name != null);
    },
    unknownDistractorTags() {
      return this.tags.filter((tag: TagItem) => tag.is_distractor === true && tag.name === null);
    }
  },
  mounted() {
    this.fetchTagIds();
    setInterval(this.fetchTagIds, 1000);
  },
  methods: {
    navigateToManageTag() {
      this.$router.push("/ManageTag");
    },
    async fetchTagIds() {
      try {
        const response = await axios.post("/api/getScanResult");
        this.tags = response.data.tags;
      }
      catch (error) {
        console.error("Error fetching tag IDs:", error);
      }
    },
  }
};
</script>
