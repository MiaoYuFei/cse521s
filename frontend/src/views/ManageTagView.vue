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
              class="bi bi-gear"
              style="color: rgb(94, 94, 94);"
            />Tag Manager
          </h1>
          <div class="card-text">
            <form>
              <div class="mb-3">
                <label
                  for="idNewTagId"
                  class="form-label"
                >Tag ID</label>
                <input
                  id="idNewTagId"
                  v-model="new_tag_id"
                  type="text"
                  class="form-control"
                  required
                >
              </div>
              <div class="mb-3">
                <label
                  for="idNewName"
                  class="form-label"
                >Tag Name</label>
                <input
                  id="idNewName"
                  v-model="new_name"
                  type="text"
                  class="form-control"
                  required
                >
              </div>
              <div
                class="mb-3"
                style="max-width: 16em;"
              >
                <label
                  for="idNewName"
                  class="form-label"
                >Is it distractor</label>
                <div
                  class="btn-group form-control"
                  role="group"
                  aria-label="Is it a distractor"
                >
                  <input
                    id="idNewIsDistractor"
                    type="radio"
                    class="btn-check"
                    name="btnradio"
                    autocomplete="off"
                    @click="new_is_distractor = true;"
                  >
                  <label
                    class="btn btn-outline-primary"
                    for="idNewIsDistractor"
                  >Yes</label>

                  <input
                    id="idNewNotDistractor"
                    type="radio"
                    class="btn-check"
                    name="btnradio"
                    autocomplete="off"
                    checked
                    @click="new_is_distractor = false;"
                  >
                  <label
                    class="btn btn-outline-primary"
                    for="idNewNotDistractor"
                  >No</label>
                </div>
              </div>

              <div class="mb-3">
                <button
                  type="button"
                  class="btn btn-primary"
                  @click="addTag()"
                >
                  Add
                </button>
              </div>
            </form>
            <hr>
            <table class="table table-bordered table-striped">
              <thead>
                <th>Tag Id</th>
                <th>Name</th>
                <th>Is Distractor</th>
                <th>Operations</th>
              </thead>
              <tbody>
                <TagTableRow
                  v-for="item in tagList"
                  :key="item.tag_id"
                  :item="item"
                  @save="saveChanges"
                  @delete="deleteTag"
                />
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BsNavbar from "@/components/BsNavbar.vue";
import TagTableRow from "@/components/TagTableRow.vue";
import axios from "axios";

interface TagItem {
  tag_id: string;
  name: string;
  is_distractor: boolean;
}

export default {
  name: "ManageTagView",
  components: { BsNavbar, TagTableRow },
  data() {
    return {
      new_tag_id: "",
      new_name: "",
      new_is_distractor: false,
      tagList: [] as TagItem[],
    };
  },
  mounted() {
    this.fetchAllTags();
  },
  methods: {
    fetchAllTags() {
      axios.post('/api/getAllTags').then((response) => {
        if (response.data.success) {
          this.tagList = [];
          response.data.tags.forEach((element: { tag_id: string; name: string; is_distractor: string; }) => {
            (this.tagList as TagItem[]).push({
              tag_id: element.tag_id,
              name: element.name,
              is_distractor: element.is_distractor === "true" ? true : false
            } as TagItem);
          });
        } else {
          console.error("API response indicates failure");
        }
      });
    },

    saveChanges(updatedItem: TagItem) {
      if (updatedItem.name === null || updatedItem.name === "") {
        alert("Tag name can not be empty!");
        return false;
      }
      const backendURL = "/api/editTag";
      const updatedData = {
        tag_id: updatedItem.tag_id,
        name: updatedItem.name,
        is_distractor: updatedItem.is_distractor,
      };
      axios
        .post(backendURL, updatedData)
        .then((response) => {
          if (response.data.success) {
            console.log('Tag updated successfully');
            this.fetchAllTags();
          } else {
            console.error('API response indicates failure');
          }
        })
        .catch((error) => {
          console.error('Error updating tag:', error);
        });
      console.log(updatedItem);
    },

    deleteTag(tag_id: string) {
      const backendURL = "/api/deleteTag";
      const dataToSend = {
        tag_id: tag_id,
      };
      axios
        .post(backendURL, dataToSend)
        .then((response) => {
          if (response.data.success) {
            this.fetchAllTags();
          } else {
            console.error("API response indicates failure");
          }
        })
        .catch((error) => {
          console.error('Error sending data to the backend:', error);
        });
    },

    addTag() {
      if (this.new_tag_id === null || this.new_tag_id === "" ||
        this.new_name === null || this.new_name === "") {
        alert("Tag information can not be empty!");
        return false;
      }
      const backendURL = "/api/addTag";
      const dataToSend = {
        tag_id: this.new_tag_id,
        name: this.new_name,
        is_distractor: this.new_is_distractor ? "true" : "false",
      };

      axios
        .post(backendURL, dataToSend)
        .then((response) => {
          console.log("Backend response:", response.data);
          if (response.data.success) {
            this.new_tag_id = "";
            this.new_name = "";
            this.new_is_distractor = false;
            document.getElementById("idNewNotDistractor")?.click();
            this.fetchAllTags();
          } else {
            console.error("API response indicates failure");
          }
        })
        .catch((error) => {
          console.error("Error sending data to the backend:", error);
        });
    }
  },
};
</script>
